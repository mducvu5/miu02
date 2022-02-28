using System.Reflection;
using System.Runtime.Loader;

public class SnekAssemblyLoadContext : AssemblyLoadContext
{
    public SnekAssemblyLoadContext()
        : base(isCollectible: true)
    {
    }

    protected override Assembly? Load(AssemblyName name)
        => null;
}