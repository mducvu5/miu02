using Discord;

namespace Nadeko.Snake;

public abstract class AnyContext
{
    public abstract IMessageChannel Channel { get; }
    public abstract IUserMessage Message { get; }
}