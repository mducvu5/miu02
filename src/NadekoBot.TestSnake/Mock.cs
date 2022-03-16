using Discord;
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
        await ctx.ReplyConfirmLocalizedAsync("ok_reply"); 
        await Task.Delay(1500);
        await ctx.ReplyErrorLocalizedAsync("error_reply");
        await Task.Delay(1500);
        await ctx.ReplyPendingLocalizedAsync("pending_reply");
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

    // [cmd]
    // public async Task Type(GuildContext ctx, MyType type)
    // {
    //     await ctx.SendConfirmAsync(type.Text);
    // }

    public override ValueTask<bool> ExecOnMessageAsync(IGuild? guild, IUserMessage msg)
    {
        return new(false);
    }

    public override ValueTask<string?> ExecInputTransformAsync(IGuild? guild, IMessageChannel channel, IUser user, string input)
    {
        return new(default(string));
    }

    public override ValueTask<bool> ExecPreCommandAsync(AnyContext context, string moduleName, string commandName)
    {
        return new(false);
    }

    public override ValueTask ExecPostCommandAsync(AnyContext ctx, string moduleName, string commandName)
    {
        return default;
    }

    public override ValueTask ExecOnNoCommandAsync(IGuild? guild, IUserMessage msg)
    {
        return default;
    }

    public override ValueTask InitializeAsync()
    {
        return default;
    }

    public override ValueTask DisposeAsync()
    {
        return default;
    }
}

public class MyType
{
    public string Text { get; init; } = string.Empty;
}

public struct MyValue
{
    public string Text { get; set; }
}

public sealed class MyTypeParamParser : ParamParser<MyType>
{
    private readonly SewuisSingleton _svc;

    public MyTypeParamParser(SewuisSingleton svc)
    {
        _svc = svc;
    }
    
    public override ValueTask<ParseResult<MyType>> TryParseAsync(AnyContext ctx, string data)
    {
        if (data.Contains("0"))
            return new(ParseResult<MyType>.Success(new()
            {
                Text = data
            }));

        return new(ParseResult<MyType>.Fail());
    }
}