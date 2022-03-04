using Nadeko.Snake;

public sealed class GuildContextAdapter : GuildContext
{
    private readonly ICommandContext _ctx;

    public GuildContextAdapter(ICommandContext ctx)
    {
        if (ctx.Guild is not IGuild guild || ctx.Channel is not ITextChannel channel)
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

public sealed class DmContextAdapter : DmContext
{
    public override IDMChannel Channel { get; }
    public override IUserMessage Message { get; }

    public DmContextAdapter(ICommandContext ctx)
    {
        if (ctx is not { Channel: IDMChannel ch })
        {
            throw new ArgumentException("Can't use non-dm context to create DmContextAdapter", nameof(ctx));
        }

        Channel = ch;
        Message = ctx.Message;
    }
}