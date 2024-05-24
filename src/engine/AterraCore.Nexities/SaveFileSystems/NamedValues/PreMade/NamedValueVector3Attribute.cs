// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.SaveFileSystem.NamedValues;
namespace AterraCore.Nexities.SaveFileSystems.NamedValues.PreMade;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class NamedValueVector3Attribute(string? name = null) : NamedValueAttribute(name, NamedValueConvertors.ToVector3);