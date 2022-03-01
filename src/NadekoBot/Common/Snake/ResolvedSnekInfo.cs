public record ResolvedSnekInfo(
    WeakReference<SnekAssemblyLoadContext> LoadContext,
    IReadOnlyCollection<ModuleInfo> ModuleInfos,
    IReadOnlyCollection<SnekData> SnekInfos
);