namespace Nadeko.Snake;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
public abstract class FilterAttribute : Attribute
{
    public abstract ValueTask<bool> CheckAsync(AnyContext ctx);
}