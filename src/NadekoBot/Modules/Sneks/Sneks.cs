namespace NadekoBot.Modules;

public partial class Sneks : NadekoModule<ISnekLoaderService>
{
    [Cmd]
    public async partial Task Load(string name)
    {
        if (await _service.LoadSnekAsync(name))
            await ctx.OkAsync();
        else
            await ctx.ErrorAsync();
    }
    
    [Cmd]
    public async partial Task Unload(string name)
    {
        var succ = await _service.UnloadSnekAsync(name);
        if (succ)
            await ctx.OkAsync();
        else
            await ctx.ErrorAsync();
    }
}