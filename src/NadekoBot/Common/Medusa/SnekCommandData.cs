using Nadeko.Snake;
using System.Collections.Immutable;
using System.Reflection;

public sealed class SnekCommandData
{
    public SnekCommandData(
        IReadOnlyCollection<string> aliases,
        MethodInfo methodInfo,
        Snek module,
        FilterAttribute[] filters,
        CommandContextType contextType,
        IReadOnlyList<Type> injectedParams,
        IReadOnlyList<ParamData> parameters,
        int priority)
    {
        Aliases = aliases;
        MethodInfo = methodInfo;
        Module = module;
        Filters = filters;
        ContextType = contextType;
        InjectedParams = injectedParams;
        Parameters = parameters;
        Priority = priority;
    }

    public IReadOnlyCollection<string> Aliases { get; }
    public MethodInfo MethodInfo { get; set; }
    public Snek Module { get; set; }
    public FilterAttribute[] Filters { get; set; }
    public CommandContextType ContextType { get; }
    public IReadOnlyList<Type> InjectedParams { get; }
    public IReadOnlyList<ParamData> Parameters { get; }
    public int Priority { get; }
}