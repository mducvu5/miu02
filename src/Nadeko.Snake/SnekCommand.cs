namespace Nadeko.Snake;

[AttributeUsage(AttributeTargets.Method)]
public class SnekCommand : Attribute
{
    public string Name { get; }

    public SnekCommand(string name)
        => Name = name;
}