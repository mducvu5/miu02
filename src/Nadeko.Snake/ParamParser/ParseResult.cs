namespace Nadeko.Snake;

public readonly struct ParseResult<T>
{
    public bool Success { get; private init; }
    public T? Object { get; private init;  }

    public static ParseResult<T> Fail()
        => new ParseResult<T>
        {
            Success = false,
            Object = default,
        };

    public static ParseResult<T> FromSuccess(T obj)
        => new ParseResult<T>
        {
            Success = true,
            Object = obj,
        };
}