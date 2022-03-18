namespace Nadeko.Medusa;

public sealed record SnekData(
    string Name,
    SnekData? Parent,
    Snek Instance,
    IReadOnlyCollection<SnekCommandData> Commands,
    IReadOnlyCollection<FilterAttribute> Filters)
{
    public List<SnekData> Subsneks { get; set; } = new();
}