using Discord.Commands.Builders;
using Microsoft.Extensions.DependencyInjection;
using Nadeko.Snake;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;
using PriorityAttribute = Nadeko.Snake.PriorityAttribute;

public class SnekLoaderService : ISnekLoaderService, INService
{
    private readonly CommandService _cmdService;
    private readonly Dictionary<string, ResolvedSnekInfo> _loaded = new();

    public SnekLoaderService(CommandService cmdService)
        => _cmdService = cmdService;

    [MethodImpl(MethodImplOptions.NoInlining)]
    public async Task<bool> LoadSnekAsync(string name)
    {
        if (_loaded.ContainsKey(name))
            return false;
        
        var safeName = Uri.EscapeDataString(name);
        var path = $"sneks/{safeName}/{safeName}.dll";
        name = name.ToLowerInvariant();
        
        if (LoadAssemblyInternal(path, out var ctx, out var snekData))
        {
            var moduleInfos = new List<ModuleInfo>();
            foreach (var point in snekData)
            {
                var module = await LoadModuleInternalAsync(point);
                moduleInfos.Add(module);
            }

            _loaded[name] = new(LoadContext: ctx, ModuleInfos: moduleInfos, SnekInfos: snekData);
            return true;
        }
        
        return false;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private bool LoadAssemblyInternal(string path, 
        [NotNullWhen(true)] out WeakReference<SnekAssemblyLoadContext>? ctxWr,
        [NotNullWhen(true)] out IReadOnlyCollection<SnekData>? snekData
        )
    {
        var ctx = new SnekAssemblyLoadContext();
        var a = ctx.LoadFromAssemblyPath(Path.GetFullPath(path));

        var sis = LoadSneksFromAssembly(a);

        if (sis.Count == 0)
        {
            ctxWr = null;
            snekData = null;
            
            return false;
        }

        ctxWr = new(ctx);
        snekData = sis;
        
        return true;
    }
    
    [MethodImpl(MethodImplOptions.NoInlining)]
    private async Task<ModuleInfo> LoadModuleInternalAsync(SnekData snekInfo)
    {
        var module = await _cmdService.CreateModuleAsync(string.Empty,
            CreateModuleFactory(snekInfo));
        
        return module;
    }

    private Action<ModuleBuilder> CreateModuleFactory(SnekData snekInfo)
        => mb =>
        {
            var m = mb.WithName(snekInfo.Name);

            foreach (var cmd in snekInfo.Commands)
            {
                m.AddCommand(cmd.Aliases.First(),
                    CreateCallback(new(cmd.MethodInfo), cmd.ContextType, new(snekInfo.Instance)),
                    CreateCommandFactory(cmd));
            }

            foreach (var subInfo in snekInfo.Submodules)
                m.AddModule(string.Empty, CreateModuleFactory(subInfo));
        };

    private static readonly RequireContextAttribute _reqGuild = new RequireContextAttribute(ContextType.Guild);
    private static readonly RequireContextAttribute _reqDm = new RequireContextAttribute(ContextType.DM);
    private Action<CommandBuilder> CreateCommandFactory(SnekCommandData cmd)
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
        WeakReference<MethodInfo> methodInfoWr,
        CommandContextType contextType,
        WeakReference<Snek> snekInstance)
        => async (context, parameters, svcs, _) =>
        {
            if (!methodInfoWr.TryGetTarget(out var methodInfo) || !snekInstance.TryGetTarget(out var snek))
            {
                Log.Warning("Attempted to run an unloaded snek's command");
                return;
            }
                
            var offset = contextType == CommandContextType.Unspecified ? 0 : 1;
            var paramObjs = parameters;

            if (offset == 1)
            {
                paramObjs = new object[parameters.Length + offset];

                paramObjs[0] = context.Guild is null
                    ? new DmContextAdapter(context)
                    : new GuildContextAdapter(context);
                
                for (var i = 0; i < parameters.Length; i++)
                {
                    paramObjs[i + offset] = parameters[i];
                }
            }

            using var scope = svcs.CreateScope();
            if (methodInfo.ReturnType == typeof(Task)
                || (methodInfo.ReturnType.IsGenericType
                    && methodInfo.ReturnType.GetGenericTypeDefinition() == typeof(Task<>)))
            {
                await (Task)methodInfo.Invoke(snek, paramObjs)!;
            }
            else if (methodInfo.ReturnType == typeof(ValueTask))
            {
                await (ValueTask)methodInfo.Invoke(snek, paramObjs)!;
            }
            else if (methodInfo.ReturnType == typeof(void))
            {
                methodInfo.Invoke(snek, paramObjs);
            }
        };
    
    [MethodImpl(MethodImplOptions.NoInlining)]
    public async Task<bool> UnloadSnekAsync(string name)
    {
        name = name.ToLowerInvariant();
        if (!_loaded.Remove(name, out var lsi))
            return false;

        foreach (var mi in lsi.ModuleInfos)
        {
            await _cmdService.RemoveModuleAsync(mi);
        }

        var lc = lsi.LoadContext;
        
        // removing this line will prevent assembly from being unloaded quickly
        // as this local variable will be held for a long time potentially
        // due to how async works
        lsi = null;
        return UnloadInternal(lc);
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

    private bool UnloadInternal(WeakReference<SnekAssemblyLoadContext> lsi)
    {
        UnloadContext(lsi);
        GcCleanup();

        return !lsi.TryGetTarget(out _);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private void UnloadContext(WeakReference<SnekAssemblyLoadContext> lsiLoadContext)
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
        }
    }

    private static readonly Type _snekType = typeof(Snek);  
    
    [MethodImpl(MethodImplOptions.NoInlining)]
    public IReadOnlyCollection<SnekData> LoadSneksFromAssembly(Assembly a)
    {
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
        
        foreach (var c in classes)
        {
            if (c.DeclaringType is not null)
                continue;
            
            // get module data, and add it to the topModules dictionary
            var module = GetModuleData(c);
            topModules.Add(c, module);
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
            
            GetModuleData(c, parentData);
        }

        return topModules.Values.ToArray();
    }
    
    [MethodImpl(MethodImplOptions.NoInlining)]
    private SnekData GetModuleData(Type type, SnekData? parentData = null)
    {
        var filters = type.GetCustomAttributes<FilterAttribute>(true)
                          .ToArray();

        var instance = (Snek)Activator.CreateInstance(type)!;
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
                              if(x.GetCustomAttribute<Command>(true) is null)
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
            var prio = method.GetCustomAttribute<PriorityAttribute>()?.Priority ?? 0;

            var paramInfos = method.GetParameters();
            var cmdParams = new List<ParamData>();
            var cmdContext = CommandContextType.Unspecified;
            for (var paramCounter = 0; paramCounter < paramInfos.Length; paramCounter++)
            {
                var pi = paramInfos[paramCounter];

                var paramName = pi.Name;
                var isContext = paramCounter == 0 && pi.ParameterType.IsAssignableTo(typeof(AnyContext));

                var leftoverAttribute = pi.GetCustomAttribute<Nadeko.Snake.LeftoverAttribute>();
                var hasDefaultValue = pi.HasDefaultValue;
                var isLeftover = leftoverAttribute != null;
                var isParams = pi.GetCustomAttribute<ParamArrayAttribute>() != null;
                var paramType = pi.ParameterType;

                if (isContext)
                {
                    if (hasDefaultValue || leftoverAttribute != null || isParams)
                        throw new ArgumentException("IContext parameter cannot be optional, leftover, constant or params. " + GetErrorPath(method, pi));

                    if (paramType.IsAssignableTo(typeof(GuildContext)))
                        cmdContext = CommandContextType.Guild;
                    else if (paramType.IsAssignableTo(typeof(DmContext)))
                        cmdContext = CommandContextType.Dm;
                    else
                        cmdContext = CommandContextType.Any;
                    
                    continue;
                }

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


            var aliases = method.GetCustomAttribute<Command>()!.Aliases;
            if (aliases.Length == 0)
                aliases = new[] { method.Name.ToLowerInvariant() };
            
            cmds.Add(new(
                aliases,
                method,
                instance,
                filters,
                cmdContext,
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