using Nadeko.Snake;
using System.Reflection;

public class SnekCommandInfo
{
    public string Name { get; set; }
    public Func<AnyContext, Task> Execute { get; set; }
    public MethodInfo Method { get; set; }
}