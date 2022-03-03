using Discord;
using Nadeko.Snake;

namespace NadekoBot.TestSnake;

public class Papa : Snek
{
    public override string Name
        => "papa";

    [Command()]
    public Task Out()
        => Task.CompletedTask;

    public class Uwu : Snek
    {
        // private readonly MyService _svc;

        public override string Name
            => "uwu";


        public override ValueTask InitializeAsync()
        {
            Console.WriteLine("initializing");
            return default;
        }

        public override ValueTask DisposeAsync()
        {
            Console.WriteLine("Cleaning up");
            return default;
        }

        // public Uwu()
        // {
        //     _svc = new MyService();
        // }

        [Command]
        public void Void(GuildContext ctx)
        {
            Console.WriteLine("void");
        }
        
        [Command]
        public Task Task(GuildContext ctx)
        {
            return ctx.Channel.SendMessageAsync("Task");
        }
        
        [Command]
        public async Task<int> Taskt(GuildContext ctx)
        {
            await ctx.Channel.SendMessageAsync("Hello taskt");
            return 1;
        }
        
        [Command]
        public async Task Owo(GuildContext ctx)
        {
            await ctx.Channel.SendMessageAsync(
                $"Basic owo command!! Does unloading work after a few horus? Just rebuild!!");
        }

        [Command]
        public async Task par(AnyContext ctx, string a, int b, [Leftover] IGuildUser? c = null)
        {
            await ctx.Channel.SendMessageAsync($@"This command has 3 parameters, last one being optional and leftover:
string a = {a}
int b = {b}
IGuildUser c = {c}");
        }

        [Command]
        [Priority(1)]
        public async Task Noctx()
        {
            Console.WriteLine("No context");
        }

        [Command("al", "ali", "alia")]
        public async Task Aloo(AnyContext ctx)
        {
            await ctx.Channel.SendMessageAsync($"This command should work with al, ali and alia aliases");
        }
        
        [Command("ctx")]
        [Priority(1)]
        public async Task Ctx(GuildContext ctx)
        {
            await ctx.Channel.SendMessageAsync($"Server context {ctx.Guild.Name}");
        }

        [Command("ctx")]
        [Priority(0)]
        public async Task Ctx(AnyContext ctx)
        {
            await ctx.Channel.SendMessageAsync($"Any context");
        }

        [Command("ctx")]
        [Priority(1)]
        public async Task Ctx(DmContext ctx)
        {
            await ctx.Channel.SendMessageAsync($"DM context");
        }
    }
}