public class ResolvedSnekInfo
{
    public WeakReference<SnekAssemblyLoadContext> LoadContext { get; set; }
    public SnekInfo SnekInfo { get; set; }
    public ModuleInfo ModuleInfo { get; set; }
}