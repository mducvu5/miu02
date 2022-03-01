public sealed class ParamData
{
    public ParamData(Type type,
        string name,
        bool isOptional,
        bool isLeftover,
        bool isParams)
    {
        Type = type;
        Name = name;
        IsOptional = isOptional;
        IsLeftover = isLeftover;
        IsParams = isParams;
    }

    public Type Type { get; init; }
    public string Name { get; init; }
    public bool IsOptional { get; init; }
    public bool IsLeftover { get; init; }
    public bool IsParams { get; init; }
}