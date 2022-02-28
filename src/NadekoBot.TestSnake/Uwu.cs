using Discord;
using Nadeko.Snake;

namespace NadekoBot.TestSnake;

// public class MyService
// {
//     public string GetSTring()
//         => "service string";
// }

public class Uwu : Snek
{
    // private readonly MyService _svc;

    public override string Name
        => "uwu";

    // public Uwu()
    // {
    //     _svc = new MyService();
    // }
    
    [SnekCommand("owo")]
    public async Task Owo(GuildContext ctx)
    {
        // all you need is a single DLL
        await ctx.Channel.SendMessageAsync($"Hello I'm streaming");
    }

    // only in guild
    [SnekCommand("huh")]
    public async Task Huh(GuildContext ctx)
    {
        // await ctx.Channel.SendMessageAsync(_svc.GetSTring());

        await ctx.Channel.SendMessageAsync($"Server context {ctx.Guild.Name}");
    }
}

