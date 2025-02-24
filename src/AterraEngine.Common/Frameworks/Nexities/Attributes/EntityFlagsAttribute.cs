// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraEngine.Frameworks.Nexities;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[AttributeUsage(AttributeTargets.Class)]
public class EntityFlagsAttribute(EntityFlag flags) : Attribute {
    public EntityFlag Flags { get; } = flags;
}

// TODO move this out of the file
[Flags]
public enum EntityFlag : ulong {
    Undefined = 0
}
