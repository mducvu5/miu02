#nullable disable
using Microsoft.Extensions.DependencyInjection;
using NadekoBot.Common.ModuleBehaviors;

namespace NadekoBot.Services;

public interface ICustomBehavior : IEarlyBehavior, IInputTransformer, ILateBlocker, ILateExecutor {

}

// should be renamed to handler as it's not only executing
public sealed class BehaviorExecutor : IBehaviourExecutor, INService
{
    private readonly IServiceProvider _services;
    private IReadOnlyCollection<ILateExecutor> lateExecutors;
    private IReadOnlyCollection<ILateBlocker> lateBlockers;
    private IReadOnlyCollection<IEarlyBehavior> earlyBehaviors;
    private IReadOnlyCollection<IInputTransformer> transformers;

    private readonly SemaphoreSlim _customLock = new(1, 1);
    private readonly List<ICustomBehavior> _customBehaviors = new();

    public BehaviorExecutor(IServiceProvider services)
        => _services = services;

    public void Initialize()
    {
        lateExecutors = _services.GetServices<ILateExecutor>().ToArray();
        lateBlockers = _services.GetServices<ILateBlocker>().ToArray();
        earlyBehaviors = _services.GetServices<IEarlyBehavior>().OrderByDescending(x => x.Priority).ToArray();
        transformers = _services.GetServices<IInputTransformer>().ToArray();
    }

    #region Add/Remove

    public async Task<bool> AddBehaviorAsync(ICustomBehavior behavior)
    {
        await _customLock.WaitAsync();
        try
        {
            if (_customBehaviors.Contains(behavior))
                return false;

            _customBehaviors.Add(behavior);
            return true;
        }
        finally
        {
            _customLock.Release();
        }
    }
    
    public async Task<bool> RemoveBehavior(ICustomBehavior behavior)
    {
        await _customLock.WaitAsync();
        try
        {
            return _customBehaviors.Remove(behavior);
        }
        finally
        {
            _customLock.Release();
        }
    }

    #endregion
    
    #region Running
    
    public async Task<bool> RunEarlyBehavioursAsync(SocketGuild guild, IUserMessage usrMsg)
    {
        async Task<bool> Exec<T>(IReadOnlyCollection<T> execs) where T : IEarlyBehavior
        {
            foreach (var beh in execs)
            {
                if (await beh.RunBehavior(guild, usrMsg))
                    return true;
            }

            return false;
        }

        if (await Exec(earlyBehaviors))
        {
            return true;
        }
        
        await _customLock.WaitAsync();
        try
        {
            if (await Exec(_customBehaviors))
                return true;
        }
        finally
        {
            _customLock.Release();
        }

        return false;
    }

    public async Task<bool> RunLateBlockersAsync(ICommandContext ctx, CommandInfo cmd)
    {
        async Task<bool> Exec<T>(IReadOnlyCollection<T> execs) where T: ILateBlocker
        {
            foreach (var exec in execs)
            {
                if (await exec.TryBlockLate(ctx, cmd.Module.GetTopLevelModule().Name, cmd))
                {
                    Log.Information("Late blocking User [{User}] Command: [{Command}] in [{Module}]",
                        ctx.User,
                        cmd.Aliases[0],
                        exec.GetType().Name);
                    return true;
                }
            }

            return false;
        }

        if (await Exec(lateBlockers))
            return true;

        await _customLock.WaitAsync();
        try
        {
            if (await Exec(_customBehaviors))
                return true;
        }
        finally
        {
            _customLock.Release();
        }

        return false;
    }

    public async Task RunLateExecutorsAsync(SocketGuild guild, IUserMessage usrMsg)
    {
        async Task Exec<T>(IReadOnlyCollection<T> execs) where T : ILateExecutor
        {
            foreach (var exec in execs)
            {
                try
                {
                    await exec.LateExecute(guild, usrMsg);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Error in {TypeName} late executor: {ErrorMessage}", exec.GetType().Name, ex.Message);
                }
            }
        }

        await Exec(lateExecutors);
        
        await _customLock.WaitAsync();
        try
        {
            await Exec(_customBehaviors);
        }
        finally
        {
            _customLock.Release();
        }
    }

    public async Task<string> RunInputTransformersAsync(SocketGuild guild, IUserMessage usrMsg)
    {
        async Task<string> Exec<T>(IReadOnlyCollection<T> execs, string s)
            where T : IInputTransformer
        {
            foreach (var exec in execs)
            {
                string newContent;
                if ((newContent = await exec.TransformInput(guild, usrMsg.Channel, usrMsg.Author, s))
                    != s)
                {
                    return newContent;
                }
            }

            return null;
        }

        var newContent = await Exec(transformers, usrMsg.Content);
        if (newContent is not null)
            return newContent;
        
        await _customLock.WaitAsync();
        try
        {
            newContent = await Exec(_customBehaviors, usrMsg.Content);
            if (newContent is not null)
                return newContent;
        }
        finally
        {
            _customLock.Release();
        }

        return usrMsg.Content;
    }
    
    #endregion
}