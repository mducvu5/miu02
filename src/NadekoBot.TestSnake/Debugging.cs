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
        Log.Information("instantiated transient services!! Reloaded");
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

    [cmd]
    public Task Stats(GuildContext ctx, int x)
        => ctx.Channel.SendMessageAsync($"This is my own stats {x}");

    [cmd]
    public async Task Singleton(GuildContext ctx, [Inject] SewuisSingleton sin)
    {
        sin.Sin();
        await ctx.Channel.SendMessageAsync("ok");
    }
    
    [cmd]
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

        [cmd]
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

        [cmd]
        public void Void(GuildContext ctx)
        {
            Console.WriteLine("void");
        }
        
        [cmd]
        public Task Task(GuildContext ctx)
        {
            return ctx.Channel.SendMessageAsync("Task");
        }
        
        [cmd]
        public async Task<int> Taskt(GuildContext ctx)
        {
            await ctx.Channel.SendMessageAsync("Hello taskt");
            return 1;
        }
        
        [cmd]
        public async Task Owo(GuildContext ctx)
        {
            await ctx.Channel.SendMessageAsync(
                $"Basic owo command!! Does unloading work after a few horus? Just rebuild!!");
        }

        [cmd]
        public async Task par(AnyContext ctx, string a, int b, [Leftover] IGuildUser? c = null)
        {
            await ctx.Channel.SendMessageAsync($@"This command has 3 parameters, last one being optional and leftover:
string a = {a}
int b = {b}
IGuildUser c = {c}");
        }

        [cmd]
        [prio(1)]
        public ValueTask Noctx()
        {
            Console.WriteLine("No context");
            return default;
        }

        [cmd("al", "ali", "alia")]
        public async Task Aloo(AnyContext ctx)
        {
            await ctx.Channel.SendMessageAsync($"This command should work with al, ali and alia aliases");
        }
        
        [cmd("ctx")]
        [prio(1)]
        public async Task Ctx(GuildContext ctx)
        {
            await ctx.Channel.SendMessageAsync($"Server context {ctx.Guild.Name}! Improved!!!");
        }

        [cmd("ctx")]
        [prio(0)]
        public async Task Ctx(AnyContext ctx)
        {
            await ctx.Channel.SendMessageAsync($"Any context");
        }

        [cmd("ctx")]
        [prio(1)]
        public async Task Ctx(DmContext ctx)
        {
            await ctx.Channel.SendMessageAsync($"DM context");
        }
    }
}