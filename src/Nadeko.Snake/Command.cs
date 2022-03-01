namespace Nadeko.Snake;

[AttributeUsage(AttributeTargets.Method)]
public class Command : Attribute
{
    public string[] Aliases { get; }

    public Command(params string[] aliases)
    {
        Aliases = aliases;
    }
}