using Discord;

namespace Nadeko.Snake;

public abstract class DmContext : AnyContext
{
    public abstract override IDMChannel Channel { get; }
}