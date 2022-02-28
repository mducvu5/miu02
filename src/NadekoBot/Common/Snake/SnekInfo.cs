using Nadeko.Snake;

public class SnekInfo
{
    public string Name { get; set; }
    public IReadOnlyCollection<SnekCommandInfo> Commands { get; set; }
    public Snek Instance { get; set; }
}