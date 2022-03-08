using Discord;

namespace Nadeko.Snake;
// todo medusae/medusae.yml

/// <summary>
/// Any snek has to inherit from this class.
/// The classes will that inherit instantiated ONLY ONCE during the loading,
/// and any commands will be executed on the same instance.
/// </summary>
public abstract class Snek : IAsyncDisposable
{
    /// <summary>
    /// Name of the snek. Defaults to 
    /// </summary>
    public virtual string Name
        => GetType().Name.ToLowerInvariant();

    /// <summary>
    /// The prefix required before the command name. For example
    /// if you set this to 'test' then a command called 'cmd' will have to be invoked by using
    /// '.test cmd' instead of `.cmd` 
    /// </summary>
    public virtual string Prefix
        => string.Empty;

    /// <summary>
    /// Called whenever on all received non-bot user messages
    /// </summary>
    /// <param name="msg">Received Message</param>
    /// <returns>Execution result</returns>
    public virtual ValueTask<ExecResponse> OnMessageAsync(IUserMessage msg)
        => ValueTask.FromResult<ExecResponse>(default);

    /// <summary>
    /// Called when ANY valid command (doesn't have to be from this snek) is about to be executed
    /// </summary>
    /// <param name="ctx">Context</param>
    /// <returns>Execution result</returns>
    public virtual ValueTask<ExecResponse> OnPreCommandAsync(AnyContext ctx)
        => ValueTask.FromResult<ExecResponse>(default);

    /// <summary>
    /// Called when ANY command (doesn't have to be from this snek) was executed
    /// </summary>
    /// <param name="ctx">Context</param>
    /// <returns>Execution result</returns>
    public virtual ValueTask<ExecResponse> OnPostCommandAsync(AnyContext ctx)
        => ValueTask.FromResult<ExecResponse>(default);

    /// <summary>
    /// Executed once this snek has been instantiated and before any command is executed.
    /// </summary>
    /// <returns>A <see cref="ValueTask"/> representing completion</returns>
    public virtual ValueTask InitializeAsync()
        => default;

    /// <summary>
    /// Override to cleanup any resources or references which might hold this snek in memory
    /// </summary>
    /// <returns></returns>
    public virtual ValueTask DisposeAsync()
        => default;
}

public readonly struct ExecResponse
{
    
}