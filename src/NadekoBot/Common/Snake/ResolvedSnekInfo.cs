using System.Collections.Immutable;

public record ResolvedSnekInfo(
    WeakReference<SnekAssemblyLoadContext> LoadContext,
    IImmutableList<ModuleInfo> ModuleInfos,
    IImmutableList<SnekData> SnekInfos
);