using Nadeko.Medusa;

namespace NadekoBot.Modules;

[Group("medusa")]
public partial class Medusa : NadekoModule<IMedusaLoaderService>
{
    [Cmd]
    [OwnerOnly]
    public async partial Task Load(string? name = null)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            var loaded = _service.GetLoadedMedusae()
                                 .Select(x => x.Name)
                                 .ToHashSet();
            
            var unloaded = _service.GetAllMedusae()
                    .Where(x => !loaded.Contains(x))
                    .Select(x => Format.Code(x.ToString()))
                    .ToArray();

            await ctx.SendPaginatedConfirmAsync(0,
                page =>
                {
                    return _eb.Create(ctx)
                              .WithOkColor()
                              .WithTitle(GetText(strs.list_of_unloaded))
                              .WithDescription(unloaded.Skip(10 * page).Take(10).Join('\n'));
                },
                unloaded.Length,
                10);
            return;
        }
        
        if (await _service.LoadSnekAsync(name))
            await ctx.OkAsync();
        else
            await ctx.ErrorAsync();
    }
    
    [Cmd]
    [OwnerOnly]
    public async partial Task Unload(string name = null)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            var loaded = _service.GetLoadedMedusae();
            if (loaded.Count == 0)
            {
                await ReplyErrorLocalizedAsync(strs.no_medusa_loaded);
                return;
            }

            await ctx.Channel.EmbedAsync(_eb.Create(ctx)
                                            .WithOkColor()
                                            .WithTitle(GetText(strs.loaded_medusae))
                                            .WithDescription(loaded.Select(x => x.Name)
                                                                   .Join("\n")));
            
            return;
        }
        
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
        var all = _service.GetAllMedusae();
        var loaded = _service.GetLoadedMedusae()
                             .Select(x => x.Name)
                             .ToHashSet();

        var output = all
            .Select(m =>
            {
                var emoji = loaded.Contains(m) ? "`✅`" : "`🔴`";
                return $"{emoji} `{m}`";
            })
            .ToArray();


        await ctx.SendPaginatedConfirmAsync(0,
            page => _eb.Create(ctx)
                       .WithOkColor()
                       .WithTitle(GetText(strs.list_of_medusae))
                       .WithDescription(output.Skip(page * 10).Take(10).Join('\n')),
            output.Length,
            10);
    }

    [Cmd]
    [OwnerOnly]
    public async partial Task Stats(string? name = null)
    {
        var medusae = _service.GetLoadedMedusae();

        if (name is not null)
        {
            var found = medusae.FirstOrDefault(x => string.Equals(x.Name,
                name,
                StringComparison.InvariantCultureIgnoreCase));
            
            if (found is null)
            {
                await ReplyErrorLocalizedAsync(strs.medusa_name_not_found);
                return;
            }

            var cmdCount = found.Sneks.Sum(x => x.Commands.Count);
            var cmdNames = found.Sneks
                                .SelectMany(x => x.Commands)
                                   .Select(x => Format.Code(x.Name))
                                   .Join(" | ");

            var eb = _eb.Create(ctx)
                        .WithOkColor()
                        .WithAuthor(GetText(strs.medusa_info))
                        .WithTitle(found.Name)
                        .WithDescription(found.Description)
                        .AddField(GetText(strs.sneks_count(found.Sneks.Count)),
                            found.Sneks.Count == 0
                                ? "-"
                                : found.Sneks.Select(x => x.Name).Join('\n'),
                            true)
                        .AddField(GetText(strs.commands_count(cmdCount)),
                            string.IsNullOrWhiteSpace(cmdNames)
                                ? "-"
                                : cmdNames,
                            true);

            await ctx.Channel.EmbedAsync(eb);
            return;
        }
        
        await ctx.SendPaginatedConfirmAsync(0,
            page =>
            {
                var eb = _eb.Create(ctx)
                            .WithOkColor();

                foreach (var medusa in medusae.Skip(page * 9).Take(9))
                {
                    eb.AddField(medusa.Name,
                        $@"`Sneks:` {medusa.Sneks.Count}
`Commands:` {medusa.Sneks.Sum(x => x.Commands.Count)}
--
{medusa.Description}");
                }

                return eb;
            }, medusae.Count, 9);
    }
}