namespace Nadeko.Snake;

/// <summary>
/// Higher value means higher priority
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class prioAttribute : Attribute
{
    public int Priority { get; }

    /// <summary>
    /// Snek command priority
    /// </summary>
    /// <param name="priority">Priority value. The higher the value, the higher the priority</param>
    public prioAttribute(int priority)
    {
        Priority = priority;
    }
}

[AttributeUsage(AttributeTargets.Parameter)]
public class LeftoverAttribute : Attribute
{
    
}