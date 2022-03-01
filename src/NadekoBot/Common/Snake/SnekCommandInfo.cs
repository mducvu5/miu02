using Nadeko.Snake;
using System.Reflection;

public record SnekCommandInfo(
    string Name,
    Func<AnyContext, Task> Execute,
    MethodInfo Method
);