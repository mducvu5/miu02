using Discord;
using Nadeko.Snake;

namespace NadekoBot;

public static class MedusaExtensions
{
    public static Task<IUserMessage> EmbedAsync(this IMessageChannel ch, IEmbedBuilder embed, string msg = "")
        => ch.SendMessageAsync(msg,
            embed: embed.Build(),
            options: new()
            {
                RetryMode = RetryMode.AlwaysRetry
            });

    // unlocalized
    public static Task<IUserMessage> SendConfirmAsync(this IMessageChannel ch, AnyContext ctx, string msg)
        => ch.EmbedAsync(ctx.Embed().WithOkColor().WithDescription(msg));

    public static Task<IUserMessage> SendPendingAsync(this IMessageChannel ch, AnyContext ctx, string msg)
        => ch.EmbedAsync(ctx.Embed().WithPendingColor().WithDescription(msg));

    public static Task<IUserMessage> SendErrorAsync(this IMessageChannel ch, AnyContext ctx, string msg)
        => ch.EmbedAsync(ctx.Embed().WithErrorColor().WithDescription(msg));

    // unlocalized
    public static Task<IUserMessage> SendConfirmAsync(this AnyContext ctx, string msg)
        => ctx.Channel.SendConfirmAsync(ctx, msg);

    public static Task<IUserMessage> SendPendingAsync(this AnyContext ctx, string msg)
        => ctx.Channel.SendPendingAsync(ctx, msg);

    public static Task<IUserMessage> SendErrorAsync(this AnyContext ctx, string msg)
        => ctx.Channel.SendErrorAsync(ctx, msg);

    // localized
    public static Task ConfirmAsync(this AnyContext ctx)
        => ctx.Message.AddReactionAsync(new Emoji("✅"));

    public static Task ErrorAsync(this AnyContext ctx)
        => ctx.Message.AddReactionAsync(new Emoji("❌"));

    public static Task WarningAsync(this AnyContext ctx)
        => ctx.Message.AddReactionAsync(new Emoji("⚠️"));
    
    public static Task WaitAsync(this AnyContext ctx)
        => ctx.Message.AddReactionAsync(new Emoji("🤔"));

    public static Task<IUserMessage> ErrorLocalizedAsync(this AnyContext ctx, string key, object[]? args = null)
        => ctx.SendErrorAsync(ctx.GetText(key));

    public static Task<IUserMessage> PendingLocalizedAsync(this AnyContext ctx, string key, object[]? args = null)
        => ctx.SendPendingAsync(ctx.GetText(key, args));

    public static Task<IUserMessage> ConfirmLocalizedAsync(this AnyContext ctx, string key, object[]? args = null)
        => ctx.SendConfirmAsync(ctx.GetText(key, args));
    
    public static Task<IUserMessage> ReplyErrorLocalizedAsync(this AnyContext ctx, string key, object[]? args = null)
        => ctx.SendErrorAsync($"{Format.Bold(ctx.User.ToString())} {ctx.GetText(key)}");

    public static Task<IUserMessage> ReplyPendingLocalizedAsync(this AnyContext ctx, string key, object[]? args = null)
        => ctx.SendPendingAsync($"{Format.Bold(ctx.User.ToString())} {ctx.GetText(key)}");

    public static Task<IUserMessage> ReplyConfirmLocalizedAsync(this AnyContext ctx, string key, object[]? args = null)
        => ctx.SendConfirmAsync($"{Format.Bold(ctx.User.ToString())} {ctx.GetText(key)}");
}

public static class EmbedBuilderExtensions
{
    public static IEmbedBuilder WithOkColor(this IEmbedBuilder eb)
        => eb.WithColor(EmbedColor.Ok);
    
    public static IEmbedBuilder WithPendingColor(this IEmbedBuilder eb)
        => eb.WithColor(EmbedColor.Pending);
    
    public static IEmbedBuilder WithErrorColor(this IEmbedBuilder eb)
        => eb.WithColor(EmbedColor.Error);
    
}

public interface IEmbedBuilder
{
    IEmbedBuilder WithDescription(string? desc);
    IEmbedBuilder WithTitle(string title);
    IEmbedBuilder AddField(string title, object value, bool isInline = false);
    IEmbedBuilder WithFooter(string text, string? iconUrl = null);
    IEmbedBuilder WithAuthor(string name, string? iconUrl = null, string? url = null);
    IEmbedBuilder WithColor(EmbedColor color);
    Embed Build();
    IEmbedBuilder WithUrl(string url);
    IEmbedBuilder WithImageUrl(string url);
    IEmbedBuilder WithThumbnailUrl(string url);
}

public enum EmbedColor
{
    Ok,
    Pending,
    Error
}