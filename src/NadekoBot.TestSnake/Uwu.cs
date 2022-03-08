using Discord;
using Nadeko.Snake;
using NadekoBot.Services;
using Serilog;
using StackExchange.Redis;

namespace NadekoBot.TestSnake;

[Service(Lifetime.Transient)]
public class SewuisTransient
{
    private readonly SewuisSingleton _sin;

    public SewuisTransient(SewuisSingleton sin)
    {
        _sin = sin;
        Console.WriteLine("instantiated transient services!! Reloaded");
    }

    public string Tra()
    {
        Log.Information("tra");

        return $"tra_{_sin.Sin()}";
    }
}

[Service(Lifetime.Singleton)]
public class SewuisSingleton
{
    public SewuisSingleton()
    {
        Console.WriteLine("instantiated singleton service");
    }

    public string Sin()
    {
        Log.Information("Sin");
        return "sin";
    }
}

public class Papa : Snek
{
    public override string Name
        => "papa";

    [Command]
    public Task Stats(GuildContext ctx, int x)
        => ctx.Channel.SendMessageAsync($"This is my own stats {x}");

    [Command]
    public async Task Singleton(GuildContext ctx, [Inject] SewuisSingleton sin)
    {
        sin.Sin();
        await ctx.Channel.SendMessageAsync("ok");
    }
    
    [Command]
    public async Task Transient(GuildContext ctx, [Inject] SewuisTransient tra)
    {
        tra.Tra();
        await ctx.Channel.SendMessageAsync("ok");
    }

    public class Uwu : Snek
    {
        private readonly IStatsService _stats;

        private readonly ConnectionMultiplexer _multi;
        // private readonly MyService _svc;

        public override string Name
            => "uwu";

        public override string Prefix
            => "uwu";

        public Uwu(IStatsService stats, ConnectionMultiplexer multi)
        {
            _stats = stats;
            _multi = multi;
        }

        public override ValueTask InitializeAsync()
        {
            Console.WriteLine(typeof(Scrutor.IAssemblySelector).Assembly);
            // Console.WriteLine(typeof(Ayu.Discord.Gateway.CloseCodes));
            return default;
        }

        public override ValueTask DisposeAsync()
        {
            Console.WriteLine("Cleaning up");
            return default;
        }

        [Command]
        public async Task Stats(GuildContext ctx)
        {
            await ctx.Channel.SendMessageAsync(
                $"I'm using IStatsService!, Here's the current uptime: {_stats.GetUptimeString()}");
        }

        // [Command]
        // public async Task Services(
        //     GuildContext ctx,
        //     [Inject] SewuisSingleton ss,
        //     [Inject] SewuisTransient st,
        //     int a,
        //     int b
        // )
        // {
        //     ss.Sin();
        //     Console.WriteLine("---");
        //     st.Tra();
        //     
        //     await ctx.Channel.SendMessageAsync("owo " + a + b);
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
        public ValueTask Noctx()
        {
            Console.WriteLine("No context");
            return default;
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
            await ctx.Channel.SendMessageAsync($"Server context {ctx.Guild.Name}! Improved!!!");
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