using Discord;

namespace Nadeko.Snake;

public abstract class GuildContext : AnyContext
{
   public abstract override ITextChannel Channel { get; }
   public abstract IGuild Guild { get; }
}