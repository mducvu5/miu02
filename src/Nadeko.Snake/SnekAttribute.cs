namespace Nadeko.Snake;

[AttributeUsage(AttributeTargets.Class)]
public class SnekAttribute : Attribute
{
    public string Name { get; set; }
}