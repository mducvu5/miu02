using NadekoBot.Common.Medusa;

#nullable disable
namespace NadekoBot.Common.TypeReaders;

[MeansImplicitUse(ImplicitUseTargetFlags.Default | ImplicitUseTargetFlags.WithInheritors)]
public abstract class NadekoTypeReader<T> : TypeReader
{
    public abstract ValueTask<TypeReaderResult<T>> ReadAsync(ICommandContext ctx, string input);

    public override async Task<Discord.Commands.TypeReaderResult> ReadAsync(
        ICommandContext ctx,
        string input,
        IServiceProvider services)
        => await ReadAsync(ctx, input);
}

public sealed class TypeReaderParamParserAdapter<T> : TypeReader
{
    private readonly ParamParser<T> _parser;
    private readonly IMedusaStrings _strings;
    private readonly WeakReference<IServiceProvider> _medusaServices;

    public TypeReaderParamParserAdapter(ParamParser<T> parser,
        IMedusaStrings strings,
        WeakReference<IServiceProvider> medusaServices)
    {
        _parser = parser;
        _strings = strings;
        _medusaServices = medusaServices;
    }

    public override async Task<Discord.Commands.TypeReaderResult> ReadAsync(
        ICommandContext context,
        string input,
        IServiceProvider services)
    {
        var medusaContext = ContextAdapterFactory.CreateNew(context,
            _strings,
            new MedusaServiceProvider(services, _medusaServices));
        
        var result = await _parser.TryParseAsync(medusaContext, input);
        
        if(result.Success)
            return Discord.Commands.TypeReaderResult.FromSuccess(result.Object);
        
        return Discord.Commands.TypeReaderResult.FromError(CommandError.Unsuccessful, "Invalid input");
    }
}