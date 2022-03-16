namespace Nadeko.Snake;

public abstract class ParamParser<T>
{
    public abstract ValueTask<ParseResult<T>> TryParseAsync(AnyContext ctx, string data);
}