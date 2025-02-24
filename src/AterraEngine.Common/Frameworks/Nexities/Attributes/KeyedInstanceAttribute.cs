// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraEngine.Frameworks.Nexities;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
// TODO create analyzer that checks if this is a valid Guid
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class KeyedInstanceAttribute(string guid) : Attribute {
    public Guid Guid { get; } = guid.ToGuid();
}
