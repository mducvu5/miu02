using Nadeko.Snake;

public record SnekInfo(
    string Name,
    IReadOnlyCollection<SnekCommandInfo> Commands,
    Snek Instance
);