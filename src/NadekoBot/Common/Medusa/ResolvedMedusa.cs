using System.Collections.Immutable;

public sealed record ResolvedMedusa(
    WeakReference<MedusaAssemblyLoadContext> LoadContext,
    IImmutableList<ModuleInfo> ModuleInfos,
    IImmutableList<SnekData> SnekInfos,
    IMedusaStrings Strings)
{
    public IServiceProvider Services { get; set; } = null!;
}