using Nadeko.Snake;

public sealed record SnekData(string Name,
    SnekData? Parent,
    Snek Instance,
    IReadOnlyCollection<SnekCommandData> Commands,
    IReadOnlyCollection<FilterAttribute> Filters)
{
    public List<SnekData> Submodules { get; set; } = new();
}