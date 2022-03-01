using Nadeko.Snake;
using System.Reflection;

public sealed class SnekCommandData
{
    public SnekCommandData(IReadOnlyCollection<string> aliases,
        MethodInfo methodInfo,
        Snek module,
        FilterAttribute[] filters,
        CommandContextType contextType,
        IReadOnlyCollection<ParamData> parameters,
        int priority)
    {
        Aliases = aliases;
        MethodInfo = methodInfo;
        Module = module;
        Filters = filters;
        ContextType = contextType;
        Parameters = parameters;
        Priority = priority;
    }

    public IReadOnlyCollection<string> Aliases { get; }
    public MethodInfo MethodInfo { get; set; }
    public Snek Module { get; set; }
    public FilterAttribute[] Filters { get; set; }
    public CommandContextType ContextType { get; }
    public IReadOnlyCollection<ParamData> Parameters { get; }
    public int Priority { get; }
}