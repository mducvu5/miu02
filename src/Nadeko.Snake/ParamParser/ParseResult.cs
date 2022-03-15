namespace Nadeko.Snake;

public readonly struct ParseResult<T>
{
    public bool IsSuccess { get; private init; }
    public T? Data { get; private init;  }

    // add reason in the future?
    public static ParseResult<T> Fail()
        => new ParseResult<T>
        {
            IsSuccess = false,
            Data = default,
        };

    public static ParseResult<T> Success(T obj)
        => new ParseResult<T>
        {
            IsSuccess = true,
            Data = obj,
        };
}