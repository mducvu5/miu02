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

    /// <summary>
    /// This method is called right after the message was received by the bot.
    /// You can use this method to make the bot ignore certain messages based on a condition.
    /// Or you can run custom logic without intention of blocking the further processing of the message 
    /// </summary>
    /// <param name="guild">Guild in which the message was sent</param>
    /// <param name="msg">Message received by the bot</param>
    /// <returns>Whether the message should be ignored and not processed further</returns>
    public virtual ValueTask<bool> ExecEarlyAsync(IGuild? guild, IUserMessage msg)
        => default;

    /// <summary>
    /// Override this method to modify input before the bot searches for any commands matching the input
    /// Executed after <see cref="ExecEarlyAsync"/>
    /// This is useful if you want to reinterpret the message under some conditions 
    /// </summary>
    /// <param name="guild">Guild in which the message was sent</param>
    /// <param name="channel">Channel in which the message was sent</param>
    /// <param name="user">User who sent the message</param>
    /// <param name="input">Content of the message</param>
    /// <returns>New, potentially modified content</returns>
    public virtual ValueTask<string> ExecInputTransformAsync(
        IGuild? guild,
        IMessageChannel channel,
        IUser user,
        string input
    )
        => default;
    
    /// <summary>
    /// This method is called after the command was found but not executed,
    /// and can be used to prevent the command's execution.
    /// The command information doesn't have to be from this snek as this method
    /// will be called when *any* command from any module or snek was found.
    /// You can choose to prevent the execution of the command by returning "true" value.
    /// </summary>
    /// <param name="context">Command context</param>
    /// <param name="moduleName">Name of the snek or module from which the command originates</param>
    /// <param name="commandName">Name of the command which is about to be executed</param>
    /// <returns>Whether the execution should be blocked</returns>
    public virtual ValueTask<bool> ExecLateAsync(
        AnyContext context,
        string moduleName,
        string commandName
    )
        => default;

    /// <summary>
    /// This method is called after the command was succesfully executed
    /// </summary>
    /// <returns>A <see cref="ValueTask"/> representing completion</returns>
    public virtual ValueTask ExecPostCommandAsync()
        => default;
}

public readonly struct ExecResponse
{
    
}