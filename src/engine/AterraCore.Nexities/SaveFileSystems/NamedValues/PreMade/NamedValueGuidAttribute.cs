// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.SaveFileSystem.NamedValues;
namespace AterraCore.Nexities.SaveFileSystems.NamedValues.PreMade;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class NamedValueGuidAttribute(string? name = null) : NamedValueAttribute(name, NamedValueConvertors.ToGuid);