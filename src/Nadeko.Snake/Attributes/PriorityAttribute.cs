namespace Nadeko.Snake;

/// <summary>
/// Higher value means higher priority
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class PriorityAttribute : Attribute
{
    public int Priority { get; }

    /// <summary>
    /// Snek command priority
    /// </summary>
    /// <param name="priority">Priority value. The higher the value, the higher the priority</param>
    public PriorityAttribute(int priority)
    {
        Priority = priority;
    }
}

[AttributeUsage(AttributeTargets.Parameter)]
public class LeftoverAttribute : Attribute
{
    
}