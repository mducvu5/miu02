namespace Nadeko.Snake;

[AttributeUsage(AttributeTargets.Method)]
public class cmdAttribute : Attribute
{
    public string[] Aliases { get; }

    public cmdAttribute(params string[] aliases)
    {
        Aliases = aliases;
    }
}