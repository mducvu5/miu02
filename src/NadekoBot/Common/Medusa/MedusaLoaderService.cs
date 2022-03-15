using Discord.Commands.Builders;
using Microsoft.Extensions.DependencyInjection;
using NadekoBot.Common.Medusa;
using NadekoBot.Common.TypeReaders;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;

public sealed class BehaviorAdapter : ICustomBehavior
{
    private readonly Snek s;

    // unused
    public int Priority { get; }

    public BehaviorAdapter(Snek s)
    {
        this.s = s;
    }

    public Task<bool> TryBlockLate(ICommandContext context, string moduleName, CommandInfo command)
        => s.ExecLateAsync();

    public Task<bool> RunBehavior(IGuild guild, IUserMessage msg)
        => s.ExecEarlyAsync(guild, msg).AsTask();

    public Task<string> TransformInput(
        IGuild guild,
        IMessageChannel channel,
        IUser user,
        string input)
        => s.ExecInputTransformAsync(guild, channel, user, input).AsTask();

    public Task LateExecute(IGuild guild, IUserMessage msg)
        => s.ExecPostCommandAsync()
}

// ReSharper disable RedundantAssignment
public sealed class MedusaLoaderService : IMedusaLoaderService, INService
{
    private readonly CommandService _cmdService;
    private readonly IServiceProvider _svcs;
    private readonly IBehaviourExecutor _behExecutor;
    private readonly IPubSub _pubSub;
    private readonly ConcurrentDictionary<string, ResolvedMedusa> _loaded = new();
    private readonly SemaphoreSlim _lock = new SemaphoreSlim(1, 1);

    private readonly TypedKey<string> _loadKey = new("medusa:load");
    private readonly TypedKey<string> _unloadKey = new("medusa:unload");

    public MedusaLoaderService(CommandService cmdService,
        IServiceProvider svcs,
        IBehaviourExecutor behExecutor,
        IPubSub pubSub)
    {
        _cmdService = cmdService;
        _svcs = svcs;
        _behExecutor = behExecutor;
        _pubSub = pubSub;
        
        // has to be done this way to support this feature on sharded bots
        _pubSub.Sub(_loadKey, async name => await InternalLoadSnekAsync(name));
        _pubSub.Sub(_unloadKey, async name => await InternalUnloadSnekAsync(name));
    }

    public async Task<bool> LoadSnekAsync(string moduleName)
    {
        // try loading on this shard first to see if it works
        if (await InternalLoadSnekAsync(moduleName))
        {
            // if it does publish it so that other shards can load the medusa too
            // this method will be ran twice on this shard but it doesn't matter as 
            // the second attempt will be ignored
            await _pubSub.Pub(_loadKey, moduleName);
            return true;
        }

        return false;
    }
    
    public async Task<bool> UnloadSnekAsync(string moduleName)
    {
        if (await InternalUnloadSnekAsync(moduleName))
        {
            await _pubSub.Pub(_loadKey, moduleName);
            return true;
        }

        return false;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public string[] GetCommandUsages(string medusaName, string commandName, CultureInfo culture)
    {
        if (!_loaded.TryGetValue(medusaName, out var data))
            return Array.Empty<string>();

        return data.Strings.GetCommandStrings(commandName, culture).Args;
    }
    
    [MethodImpl(MethodImplOptions.NoInlining)]
    public string GetCommandDescription(string medusaName, string commandName, CultureInfo culture)
    {
        if (!_loaded.TryGetValue(medusaName, out var data))
            return string.Empty;

        return data.Strings.GetCommandStrings(commandName, culture).Desc;
    }
    
    [MethodImpl(MethodImplOptions.NoInlining)]
    private async ValueTask<bool> InternalLoadSnekAsync(string name)
    {
        if (_loaded.ContainsKey(name))
            return false;
        
        var safeName = Uri.EscapeDataString(name);
        name = name.ToLowerInvariant();

        await _lock.WaitAsync();
        try
        {
            if (LoadAssemblyInternal(safeName,
                    out var ctx,
                    out var snekData,
                    out var services,
                    out var strings,
                    out var typeReaders))
            {
                var moduleInfos = new List<ModuleInfo>();

                LoadTypeReadersInternal(typeReaders);

                foreach (var point in snekData)
                {
                    try
                    {
                        // initialize snek and subsneks
                        await point.Instance.InitializeAsync();
                        foreach (var sub in point.Submodules)
                        {
                            await sub.Instance.InitializeAsync();
                        }

                        var module = await LoadModuleInternalAsync(name, point, strings, services);
                        moduleInfos.Add(module);
                    }
                    catch (Exception ex)
                    {
                        Log.Warning(ex,
                            "Error loading snek {SnekName}",
                            point.Name);
                    }
                }

                _loaded[name] = new(LoadContext: ctx,
                    ModuleInfos: moduleInfos.ToImmutableArray(),
                    SnekInfos: snekData.ToImmutableArray(),
                    strings,
                    typeReaders)
                {
                    Services = services
                };

                services = null;
                return true;
            }

            return false;
        }
        finally
        {
            _lock.Release();
        }
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private void LoadTypeReadersInternal(Dictionary<Type, TypeReader> typeReaders)
    {
        var notAddedTypeReaders = new List<Type>();
        foreach (var (type, typeReader) in typeReaders)
        {
            // if type reader for this type already exists, it will not be replaced
            if (_cmdService.TypeReaders.Contains(type))
            {
                notAddedTypeReaders.Add(type);
                continue;
            }

            _cmdService.AddTypeReader(type, typeReader);
        }

        // remove the ones that were not added
        // to prevent them from being unloaded later
        // as they didn't come from this medusa
        foreach (var toRemove in notAddedTypeReaders)
        {
            typeReaders.Remove(toRemove);
        }
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private bool LoadAssemblyInternal(
        string safeName,
        [NotNullWhen(true)] out WeakReference<MedusaAssemblyLoadContext>? ctxWr,
        [NotNullWhen(true)] out IReadOnlyCollection<SnekData>? snekData,
        out IServiceProvider services,
        out IMedusaStrings strings,
        out Dictionary<Type, TypeReader> typeReaders)
    {
        ctxWr = null;
        snekData = null;
        
        var path = $"medusae/{safeName}/{safeName}.dll";
        strings = MedusaStrings.CreateDefault($"medusae/{safeName}");
        var ctx = new MedusaAssemblyLoadContext(Path.GetDirectoryName(path)!);
        var a = ctx.LoadFromAssemblyPath(Path.GetFullPath(path));
        var sis = LoadSneksFromAssembly(a, out services);
        typeReaders = LoadTypeReadersFromAssembly(a, strings, new(services));

        if (sis.Count == 0)
        {
            return false;
        }

        ctxWr = new(ctx);
        snekData = sis;
        
        return true;
    }

    private static readonly Type _paramParserType = typeof(ParamParser<>);
    
    [MethodImpl(MethodImplOptions.NoInlining)]
    private Dictionary<Type, TypeReader> LoadTypeReadersFromAssembly(
        Assembly assembly,
        IMedusaStrings strings,
        WeakReference<IServiceProvider> services)
    {
        var paramParsers = assembly.GetExportedTypes()
                .Where(x => x.IsClass
                            && !x.IsAbstract
                            && x.BaseType is not null
                            && x.BaseType.IsGenericType
                            && x.BaseType.GetGenericTypeDefinition() == _paramParserType);

        var typeReaders = new Dictionary<Type, TypeReader>();
        foreach (var parserType in paramParsers)
        {
            var parserObj = ActivatorUtilities.CreateInstance(new MedusaServiceProvider(_svcs, services), parserType);

            var targetType = parserType.BaseType!.GetGenericArguments()[0];
            var typeReaderInstance = (TypeReader)Activator.CreateInstance(
                typeof(TypeReaderParamParserAdapter<>).MakeGenericType(targetType),
                args: new[] { parserObj, strings, services })!;
            
            typeReaders.Add(targetType, typeReaderInstance);
        }

        return typeReaders;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private async Task<ModuleInfo> LoadModuleInternalAsync(string medusaName, SnekData snekInfo, IMedusaStrings strings, IServiceProvider services)
    {
        var module = await _cmdService.CreateModuleAsync(snekInfo.Instance.Prefix,
            CreateModuleFactory(medusaName, snekInfo, strings, services));
        
        return module;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private Action<ModuleBuilder> CreateModuleFactory(
        string medusaName,
        SnekData snekInfo,
        IMedusaStrings strings,
        IServiceProvider medusaServices)
        => mb =>
        {
            var m = mb.WithName(snekInfo.Name);

            foreach (var cmd in snekInfo.Commands)
            {
                m.AddCommand(cmd.Aliases.First(),
                    CreateCallback(cmd.ContextType,
                        new(snekInfo),
                        new(cmd),
                        new(medusaServices),
                        strings),
                    CreateCommandFactory(medusaName, cmd));
            }

            foreach (var subInfo in snekInfo.Submodules)
                m.AddModule(subInfo.Instance.Prefix, CreateModuleFactory(medusaName, subInfo, strings, medusaServices));
        };

    private static readonly RequireContextAttribute _reqGuild = new RequireContextAttribute(ContextType.Guild);
    private static readonly RequireContextAttribute _reqDm = new RequireContextAttribute(ContextType.DM);
    private Action<CommandBuilder> CreateCommandFactory(string medusaName, SnekCommandData cmd)
        => (cb) =>
        {
            cb.AddAliases(cmd.Aliases.Skip(1).ToArray());

            if (cmd.ContextType == CommandContextType.Guild)
                cb.AddPrecondition(_reqGuild);
            else if (cmd.ContextType == CommandContextType.Dm)
                cb.AddPrecondition(_reqDm);

            cb.WithPriority(cmd.Priority);
            
            // using summary to save method name
            // method name is used to retrieve desc/usages
            cb.WithRemarks($"medusa///{medusaName}");
            cb.WithSummary(cmd.MethodInfo.Name.ToLowerInvariant());
            
            foreach (var param in cmd.Parameters)
            {
                cb.AddParameter(param.Name, param.Type, CreateParamFactory(param));
            }
        };

    private Action<ParameterBuilder> CreateParamFactory(ParamData paramData)
        => (pb) =>
        {
            pb.WithIsMultiple(paramData.IsParams)
              .WithIsOptional(paramData.IsOptional)
              .WithIsRemainder(paramData.IsLeftover);
        };

    [MethodImpl(MethodImplOptions.NoInlining)]
    private Func<ICommandContext, object[], IServiceProvider, CommandInfo, Task> CreateCallback(
        CommandContextType contextType,
        WeakReference<SnekData> snekDataWr,
        WeakReference<SnekCommandData> snekCommandDataWr,
        WeakReference<IServiceProvider> medusaServicesWr,
        IMedusaStrings strings)
        => async (context, parameters, svcs, _) =>
        {
            if (!snekCommandDataWr.TryGetTarget(out var cmdData)
                || !snekDataWr.TryGetTarget(out var snekData)
                || !medusaServicesWr.TryGetTarget(out var medusaServices))
            {
                Log.Warning("Attempted to run an unloaded snek's command");
                return;
            }
                
            var paramObjs = ParamObjs(contextType, cmdData, parameters, context, svcs, medusaServices, strings);

            try
            {
                var methodInfo = cmdData.MethodInfo;
                if (methodInfo.ReturnType == typeof(Task)
                    || (methodInfo.ReturnType.IsGenericType
                        && methodInfo.ReturnType.GetGenericTypeDefinition() == typeof(Task<>)))
                {
                    await (Task)methodInfo.Invoke(snekData.Instance, paramObjs)!;
                }
                else if (methodInfo.ReturnType == typeof(ValueTask))
                {
                    await ((ValueTask)methodInfo.Invoke(snekData.Instance, paramObjs)!).AsTask();
                }
                else // if (methodInfo.ReturnType == typeof(void))
                {
                    methodInfo.Invoke(snekData.Instance, paramObjs);
                }
            }
            finally
            {
                paramObjs = null;
                cmdData = null;
                
                snekData = null;
                medusaServices = null;
            }
        };

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static object[] ParamObjs(
        CommandContextType contextType,
        SnekCommandData cmdData,
        object[] parameters,
        ICommandContext context,
        IServiceProvider svcs,
        IServiceProvider medusaServices,
        IMedusaStrings strings)
    {
        var extraParams = contextType == CommandContextType.Unspecified ? 0 : 1;
        extraParams += cmdData.InjectedParams.Count;

        var paramObjs = new object[parameters.Length + extraParams];

        var startAt = 0;
        if (contextType != CommandContextType.Unspecified)
        {
            paramObjs[0] = ContextAdapterFactory.CreateNew(context, strings, svcs);

            startAt = 1;
        }

        var svcProvider = new MedusaServiceProvider(
            svcs,
            new(medusaServices)
        );

        for (var i = 0; i < cmdData.InjectedParams.Count; i++)
        {
            var svc = svcProvider.GetService(cmdData.InjectedParams[i]);
            if (svc is null)
            {
                throw new ArgumentException($"Cannot inject a service of type {cmdData.InjectedParams[i]}");
            }

            paramObjs[i + startAt] = svc;

            svc = null;
        }

        startAt += cmdData.InjectedParams.Count;

        for (var i = 0; i < parameters.Length; i++)
            paramObjs[startAt + i] = parameters[i];
        return paramObjs;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private async Task<bool> InternalUnloadSnekAsync(string name)
    {
        name = name.ToLowerInvariant();
        if (!_loaded.Remove(name, out var lsi))
            return false;

        await _lock.WaitAsync();
        try
        {
            // foreach (var tr in lsi.TypeReaders)
            // {
            //     _cmdService.TypeReaders
            // }

            foreach (var mi in lsi.ModuleInfos)
            {
                await _cmdService.RemoveModuleAsync(mi);
            }

            await DisposeSnekInstances(lsi);

            var lc = lsi.LoadContext;

            // removing this line will prevent assembly from being unloaded quickly
            // as this local variable will be held for a long time potentially
            // due to how async works
            // await lsi.Services.DisposeAsync();
            lsi.Services = null!;
            lsi = null;
            return UnloadInternal(lc);
        }
        finally
        {
            _lock.Release();
        }
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private async Task DisposeSnekInstances(ResolvedMedusa medusae)
    {
        foreach (var si in medusae.SnekInfos)
        {
            try
            {
                await si.Instance.DisposeAsync();
                foreach (var sub in si.Submodules)
                {
                    await sub.Instance.DisposeAsync();
                }  
            }
            catch (Exception ex)
            {
                Log.Warning(ex,
                    "Failed cleanup of Snek {SnekName}. This medusa might not unload correctly",
                    si.Instance.Name);
            }
        }

        // medusae = null;
    }

    // [MethodImpl(MethodImplOptions.NoInlining)]
    // private static void CleanupSnekData(SnekData si)
    // {
    //     si.Instance = null!;
    //     si.Filters = null!;
    //     foreach (var cmd in si.Commands)
    //     {
    //         cmd.Filters = null!;
    //         cmd.Module = null!;
    //         cmd.MethodInfo = null!;
    //         foreach (var param in cmd.Parameters)
    //         {
    //             param.Type = null!;
    //         }
    //     }
    //     
    //     foreach(var data in si.Submodules)
    //         CleanupSnekData(data);
    //
    //     si.Submodules.Clear();
    //     si.Submodules.TrimExcess();
    // }

    private bool UnloadInternal(WeakReference<MedusaAssemblyLoadContext> lsi)
    {
        UnloadContext(lsi);
        GcCleanup();

        return !lsi.TryGetTarget(out _);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private void UnloadContext(WeakReference<MedusaAssemblyLoadContext> lsiLoadContext)
    {
        if(lsiLoadContext.TryGetTarget(out var ctx))
            ctx.Unload();
    }
    
    private void GcCleanup()
    {
        // cleanup
        for (var i = 0; i < 10; i++)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.WaitForFullGCComplete();
            GC.Collect();
        }
    }

    private static readonly Type _snekType = typeof(Snek);

    [MethodImpl(MethodImplOptions.NoInlining)]
    private IServiceProvider LoadMedusaServicesInternal(Assembly a)
        => new ServiceCollection()
           .Scan(x => x.FromAssemblies(a)
                       .AddClasses(static x => x.WithAttribute<ServiceAttribute>(x => x.Lifetime == Lifetime.Transient))
                       .AsSelfWithInterfaces()
                       .WithTransientLifetime()
                       .AddClasses(static x => x.WithAttribute<ServiceAttribute>(x => x.Lifetime == Lifetime.Singleton))
                       .AsSelfWithInterfaces()
                       .WithSingletonLifetime())
           .BuildServiceProvider();
    // => new ServiceCollection()
    // {
        // var builder = new ContainerBuilder();
        // builder
        //     .RegisterAssemblyTypes(a)
        //     .PublicOnly()
        //     .Where(x => x.GetCustomAttribute<ServiceAttribute>()?.Lifetime == Lifetime.Singleton)
        //     .AsSelf()
        //     .AsImplementedInterfaces()
        //     .SingleInstance();
        //
        //
        // builder
        //     .RegisterAssemblyTypes(a)
        //     .PublicOnly()
        //     .Where(x => x.GetCustomAttribute<ServiceAttribute>()?.Lifetime == Lifetime.Transient)
        //     .AsSelf()
        //     .AsImplementedInterfaces()
        //     .InstancePerDependency();

        // builder
        //     .RegisterAssemblyTypes(a)
        //     .PublicOnly()
        //     .Where(x => x.GetCustomAttribute<ServiceAttribute>()?.Lifetime == Lifetime.Singleton)
        //     .AsSelf()
        //     .AsImplementedInterfaces()
        //     .SingleInstance();

    //     return builder.Build();
    // }


    [MethodImpl(MethodImplOptions.NoInlining)]
    public IReadOnlyCollection<SnekData> LoadSneksFromAssembly(Assembly a, out IServiceProvider services)
    {
        services = LoadMedusaServicesInternal(a);
        
        // find all types in teh assembly
        var types = a.GetExportedTypes();
        // module is always a public non abstract class
        var classes = types.Where(static x => x.IsClass
                                              && (x.IsNestedPublic || x.IsPublic)
                                              && !x.IsAbstract
                                              && x.BaseType == _snekType
                                              && (x.DeclaringType is null || x.DeclaringType.IsAssignableTo(_snekType)))
                           .ToList();

        var topModules = new Dictionary<Type, SnekData>();
        
        foreach (var cl in classes)
        {
            if (cl.DeclaringType is not null)
                continue;
            
            // get module data, and add it to the topModules dictionary
            var module = GetModuleData(cl, services);
            topModules.Add(cl, module);
        }

        foreach (var c in classes)
        {
            if (c.DeclaringType is not Type dt)
                continue;

            // if there is no top level module which this module is a child of
            // just print a warning and skip it
            if (!topModules.TryGetValue(dt, out var parentData))
            {
                Log.Warning("Can't load submodule {SubName} because parent module {Name} does not exist",
                    c.Name,
                    dt.Name);
                continue;
            }
            
            GetModuleData(c, services, parentData);
        }

        return topModules.Values.ToArray();
    }
    
    [MethodImpl(MethodImplOptions.NoInlining)]
    private SnekData GetModuleData(Type type, IServiceProvider medusaServices, SnekData? parentData = null)
    {
        var filters = type.GetCustomAttributes<FilterAttribute>(true)
                          .ToArray();
        
        var instance = (Snek)ActivatorUtilities.CreateInstance(new MedusaServiceProvider(_svcs, new(medusaServices)), type);
        
        var module = new SnekData(instance.Name,
            parentData,
            instance,
            GetCommands(instance, type),
            filters);

        if (parentData is not null)
            parentData.Submodules.Add(module);

        return module;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private IReadOnlyCollection<SnekCommandData> GetCommands(Snek instance, Type type)
    {
        var methodInfos = type
                          .GetMethods(BindingFlags.Instance
                                      | BindingFlags.DeclaredOnly
                                      | BindingFlags.Public)
                          .Where(static x =>
                          {
                              if(x.GetCustomAttribute<cmdAttribute>(true) is null)
                                  return false;

                              if (x.ReturnType.IsGenericType)
                              {
                                  var genericType = x.ReturnType.GetGenericTypeDefinition();
                                  if (genericType == typeof(Task<>))
                                      return true;
                              
                                  // if (genericType == typeof(ValueTask<>))
                                  //     return true;

                                  Log.Warning("Method {MethodName} has an invalid return type: {ReturnType}",
                                      x.Name,
                                      x.ReturnType);
                                  
                                  return false;
                              }

                              var succ = x.ReturnType == typeof(Task)
                                         || x.ReturnType == typeof(ValueTask)
                                         || x.ReturnType == typeof(void);

                              if (!succ)
                              {
                                  Log.Warning("Method {MethodName} has an invalid return type: {ReturnType}",
                                      x.Name,
                                      x.ReturnType);
                              }

                              return succ;
                          });
        
        
        var cmds = new List<SnekCommandData>();
        foreach (var method in methodInfos)
        {
            var filters = method.GetCustomAttributes<FilterAttribute>().ToArray();
            var prio = method.GetCustomAttribute<prioAttribute>()?.Priority ?? 0;

            var paramInfos = method.GetParameters();
            var cmdParams = new List<ParamData>();
            var diParams = new List<Type>();
            var cmdContext = CommandContextType.Unspecified;
            var canInject = false;
            for (var paramCounter = 0; paramCounter < paramInfos.Length; paramCounter++)
            {
                var pi = paramInfos[paramCounter];

                var paramName = pi.Name ?? "unnamed";
                var isContext = paramCounter == 0 && pi.ParameterType.IsAssignableTo(typeof(AnyContext));

                var leftoverAttribute = pi.GetCustomAttribute<Nadeko.Snake.LeftoverAttribute>(true);
                var hasDefaultValue = pi.HasDefaultValue;
                var isLeftover = leftoverAttribute != null;
                var isParams = pi.GetCustomAttribute<ParamArrayAttribute>() is not null;
                var paramType = pi.ParameterType;
                var isInjected = pi.GetCustomAttribute<InjectAttribute>(true) is not null;

                if (isContext)
                {
                    if (hasDefaultValue || leftoverAttribute != null || isParams)
                        throw new ArgumentException("IContext parameter cannot be optional, leftover, constant or params. " + GetErrorPath(method, pi));

                    if (paramCounter != 0)
                        throw new ArgumentException($"IContext parameter has to be first. {GetErrorPath(method, pi)}");

                    canInject = true;
                    
                    if (paramType.IsAssignableTo(typeof(GuildContext)))
                        cmdContext = CommandContextType.Guild;
                    else if (paramType.IsAssignableTo(typeof(DmContext)))
                        cmdContext = CommandContextType.Dm;
                    else
                        cmdContext = CommandContextType.Any;
                    
                    continue;
                }

                if (isInjected)
                {
                    if (!canInject && paramCounter != 0)
                        throw new ArgumentException($"Parameters marked as [Injected] have to come after IContext");

                    canInject = true;
                    
                    diParams.Add(paramType);
                    continue;
                }

                canInject = false;

                if (isParams)
                {
                    if (hasDefaultValue)
                        throw new NotSupportedException("Params can't have const values at the moment. "
                                                        + GetErrorPath(method, pi));
                    // if it's params, it means it's an array, and i only need a parser for the actual type,
                    // as the parser will run on each array element, it can't be null
                    paramType = paramType.GetElementType()!;
                }

                // leftover can only be the last parameter.
                if (isLeftover && paramCounter != paramInfos.Length - 1)
                {
                    var path = GetErrorPath(method, pi);
                    Log.Error("Only one parameter can be marked [Leftover] and it has to be the last one. {Path} ",
                        path);
                    throw new ArgumentException("Leftover attribute error.");
                }

                cmdParams.Add(new ParamData(paramType, paramName, hasDefaultValue, isLeftover, isParams));
            }


            var aliases = method.GetCustomAttribute<cmdAttribute>()!.Aliases;
            if (aliases.Length == 0)
                aliases = new[] { method.Name.ToLowerInvariant() };
            
            cmds.Add(new(
                aliases,
                method,
                instance,
                filters,
                cmdContext,
                diParams,
                cmdParams,
                prio
            ));
        }

        return cmds;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private string GetErrorPath(MethodInfo m, System.Reflection.ParameterInfo pi)
        => $@"Module: {m.DeclaringType?.Name} 
Command: {m.Name}
ParamName: {pi.Name}
ParamType: {pi.ParameterType.Name}";
}