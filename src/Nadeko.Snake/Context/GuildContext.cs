using Discord;

namespace Nadeko.Snake;

public abstract class GuildContext : AnyContext
{
    public abstract IGuild Guild { get; }
    public abstract override ITextChannel Channel { get; }
}