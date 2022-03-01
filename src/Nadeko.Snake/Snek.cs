using Discord;

namespace Nadeko.Snake;

// todo sneks/sneks.yml

/// <summary>
/// Any snek has to inherit from this class.
/// The classes will that inherit instantiated ONLY ONCE during the loading,
/// and any commands will be executed on the same instance.
/// </summary>
public abstract class Snek
{
    public virtual string Name 
        => GetType().Name.ToLowerInvariant(); 
    
    /// <summary>
    /// Called whenever on all received non-bot user messages
    /// </summary>
    /// <param name="msg">Received Message</param>
    /// <returns>Execution result</returns>
    public virtual ValueTask<ExecResponse> OnMessage(IUserMessage msg) => ValueTask.FromResult<ExecResponse>(default);
    
    /// <summary>
    /// Called when ANY valid command (doesn't have to be from this snek) is about to be executed
    /// </summary>
    /// <param name="ctx">Context</param>
    /// <returns>Execution result</returns>
    public virtual ValueTask<ExecResponse> OnPreCommand(AnyContext ctx) => ValueTask.FromResult<ExecResponse>(default);
    
    /// <summary>
    /// Called when ANY command (doesn't have to be from this snek) was executed
    /// </summary>
    /// <param name="ctx">Context</param>
    /// <returns>Execution result</returns>
    public virtual ValueTask<ExecResponse> OnPostCommand(AnyContext ctx) => ValueTask.FromResult<ExecResponse>(default);
}

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
public abstract class FilterAttribute : Attribute
{
    public abstract ValueTask<bool> CheckAsync(AnyContext ctx);
}

public readonly record struct ExecResponse();