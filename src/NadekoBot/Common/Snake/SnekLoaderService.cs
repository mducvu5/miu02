using Nadeko.Snake;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;

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
        
        if (LoadAssemblyInternal(path, out var loadInfo))
        { 
            var module = await LoadModuleInternalAsync(loadInfo.SnekInfo);
            loadInfo.ModuleInfo = module;
            _loaded[name] = loadInfo;
            return true;
        }
        
        return false;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private bool LoadAssemblyInternal(string path, [NotNullWhen(true)] out ResolvedSnekInfo? o)
    {
        var ctx = new SnekAssemblyLoadContext();
        var a = ctx.LoadFromAssemblyPath(Path.GetFullPath(path));

        var si = CreateSnekInfo(a);

        if (si is null)
        {
            o = null;
            return false;
        }

        o = new()
        {
            LoadContext = new(ctx),
            SnekInfo = si 
        };
        
        return true;
    }
    
    [MethodImpl(MethodImplOptions.NoInlining)]
    private async Task<ModuleInfo> LoadModuleInternalAsync(SnekInfo snekInfo)
    {
        var module = await _cmdService.CreateModuleAsync(string.Empty,
            mb =>
            {
                var m = mb.WithName(snekInfo.Name);

                foreach (var cmd in snekInfo.Commands)
                {
                    m.AddCommand(cmd.Name,
                        CreateCallback(cmd),
                        cb =>
                        {
                            cb.WithName(cmd.Name);
                        });
                }
            });

        return module;
    }

    private Func<ICommandContext, object[], IServiceProvider, CommandInfo, Task> CreateCallback(SnekCommandInfo cmd)
        => (context, parameters, svcs, cmdInfo) => cmd.Execute(new GuildContextAdapter(context));

    [MethodImpl(MethodImplOptions.NoInlining)]
    private SnekInfo? CreateSnekInfo(Assembly assembly)
    {
        var snekType = assembly
                       .GetExportedTypes()
                       .FirstOrDefault(x => x.BaseType == typeof(Snek) && !x.IsAbstract);

        if (snekType is null) return null;

        var instance = Activator.CreateInstance(snekType) as Snek;
        if (instance is null)
            return null;
        
        return new SnekInfo()
        {
            Name = instance.Name,
            Commands = CreateCommands(instance),
            Instance = instance,
        };
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private IReadOnlyCollection<SnekCommandInfo> CreateCommands(Snek instance)
    {
        var cmds = new List<SnekCommandInfo>();
        var methodInfos = instance.GetType()
                                  .GetMethods(BindingFlags.Instance
                                              | BindingFlags.DeclaredOnly
                                              | BindingFlags.Public)
                                  .Where(x => x.GetCustomAttribute<SnekCommand>(true) is not null
                                              && x.ReturnType == typeof(Task));
        
        foreach (var method in methodInfos)
        {
            cmds.Add(new SnekCommandInfo()
            {
                Name = method.GetCustomAttribute<SnekCommand>(true)!.Name,
                Method = method,
                Execute = ExecuteFactory(new(instance), new(method))
            });
        }

        return cmds;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private Func<AnyContext, Task> ExecuteFactory(WeakReference<Snek> instanceWr, WeakReference<MethodInfo> methodWr)
        => (ctx) =>
        {
            if (instanceWr.TryGetTarget(out var instance) && methodWr.TryGetTarget(out var method))
            {
                return (Task)method.Invoke(instance, new[] { ctx });
            }
            
            Log.Warning("Attempted to run an unloaded snek's command");
            return Task.CompletedTask;
        };
    
    public async Task<bool> UnloadSnekAsync(string name)
    {
        name = name.ToLowerInvariant();
        if (!_loaded.Remove(name, out var lsi))
            return false;
        
        await _cmdService.RemoveModuleAsync(lsi.ModuleInfo);

        lsi.SnekInfo.Instance = null;
        foreach (var cmd in lsi.SnekInfo.Commands)
            cmd.Method = null;
        
        return UnloadInternal(lsi);
    }

    private bool UnloadInternal(ResolvedSnekInfo lsi)
    {
        UnloadContext(lsi.LoadContext);
        GcCleanup();

        return !lsi.LoadContext.TryGetTarget(out _);
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
        for (var i = 0; i < 20; i++)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}