using Nadeko.Medusa;

namespace NadekoBot.Modules;

[Group("medusa")]
public partial class Medusa : NadekoModule<IMedusaLoaderService>
{
    [Cmd]
    [OwnerOnly]
    public async partial Task Load(string name)
    {
        if (await _service.LoadSnekAsync(name))
            await ctx.OkAsync();
        else
            await ctx.ErrorAsync();
    }
    
    [Cmd]
    [OwnerOnly]
    public async partial Task Unload(string name)
    {
        var succ = await _service.UnloadSnekAsync(name);
        if (succ)
            await ctx.OkAsync();
        else
            await ctx.ErrorAsync();
    }

    [Cmd]
    [OwnerOnly]
    public async partial Task List()
    {
        var all = _service.GetAvailableMedusae();
        var loaded = _service.GetLoadedMedusae().ToHashSet();

        var output = all
            .Select(m =>
            {
                var emoji = loaded.Contains(m) ? "`✅`" : "`🔴`";
                return $"{emoji} `{m}`";
            })
            .ToArray();


        await ctx.SendPaginatedConfirmAsync(0,
            page => _eb.Create(ctx)
                       .WithTitle(GetText(strs.list_of_medusae))
                       .WithDescription(output.Skip(page * 10).Take(10).Join('\n')),
            output.Length,
            10);
    }
}