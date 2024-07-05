// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Nexities.Data.Assets;

namespace AterraCore.Nexities.Attributes;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true)]
public class RefersToAttribute(string guid) : AbstractRefersToAttribute {
    public override Guid Guid { get; } = Guid.Parse(guid);
}
