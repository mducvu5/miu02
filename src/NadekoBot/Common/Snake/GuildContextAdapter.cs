using Nadeko.Snake;

public class GuildContextAdapter : GuildContext
{
    private readonly ICommandContext _ctx;

    public GuildContextAdapter(ICommandContext ctx)
    {
        if (!(ctx.Guild is IGuild guild && ctx.Channel is ITextChannel channel))
        {
            throw new ArgumentException("Can't use non-guild context to create GuildContextAdapter", nameof(ctx));
        }
        
        (_ctx, Guild, Channel) = (ctx, guild, channel);
    }


    public override IGuild Guild { get; }
    public override ITextChannel Channel { get; }

    public override IUserMessage Message
        => _ctx.Message;
}