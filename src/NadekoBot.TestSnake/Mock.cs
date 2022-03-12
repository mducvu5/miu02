using Nadeko.Snake;

namespace NadekoBot.TestSnake;

public class Mock : Snek
{
    public override string Name
        => "Mock";

    [cmd]
    public async Task Loc(GuildContext ctx)
    {
        // // standard, channel-based
        // await ctx.Channel.SendConfirmAsync(ctx, "Ok - plain message (ch)");
        // await Task.Delay(1500);
        // await ctx.Channel.SendPendingAsync(ctx, "Pending - plain message (ch)");
        // await Task.Delay(1500);
        // await ctx.Channel.SendErrorAsync(ctx, "Error - plain message (ch)");
        // await Task.Delay(1500);
        //
        // // standard, context-based
        // await ctx.SendConfirmAsync("Ok - plain message");
        // await Task.Delay(1500);
        // await ctx.SendPendingAsync("Pending - plain message");
        // await Task.Delay(1500);
        // await ctx.SendErrorAsync("Error - plain message");
        // await Task.Delay(1500);

        // // localized, context-based
        // await ctx.ConfirmLocalizedAsync("ok-reply");
        // await Task.Delay(1500);
        // await ctx.ErrorLocalizedAsync("error-reply");
        // await Task.Delay(1500);
        // await ctx.PendingLocalizedAsync("pending-reply");
        // await Task.Delay(1500);
        
        // localized replies, context-based
        await ctx.ReplyConfirmLocalizedAsync("ok-reply");
        await Task.Delay(1500);
        await ctx.ReplyErrorLocalizedAsync("error-reply");
        await Task.Delay(1500);
        await ctx.ReplyPendingLocalizedAsync("pending-reply");
        await Task.Delay(1500);
    }

    [cmd]
    public async Task Emoji(GuildContext ctx)
    {
        await ctx.ConfirmAsync();
        await Task.Delay(1000);
        await ctx.WaitAsync();
        await Task.Delay(1000);
        await ctx.WarningAsync();
        await Task.Delay(1000);
        await ctx.ErrorAsync();
        await Task.Delay(1000);
    }
}