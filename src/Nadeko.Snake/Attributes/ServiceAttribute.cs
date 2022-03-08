namespace Nadeko.Snake;

[AttributeUsage(AttributeTargets.Class)]
public class ServiceAttribute : Attribute
{
    public Lifetime Lifetime { get; }
    public ServiceAttribute(Lifetime lifetime = Lifetime.Singleton)
    {
        Lifetime = lifetime;
    }
}

public enum Lifetime
{
    Singleton,
    Transient
}