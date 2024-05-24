// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.SaveFileSystem.NamedValues;
namespace AterraCore.Nexities.SaveFileSystems.NamedValues;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[AttributeUsage(AttributeTargets.Property)]
public class NamedValueAttribute(string? name = null, NamedValueConvertors convertor = NamedValueConvertors.ToString) : Attribute, INamedValueAttribute {
    public string? Name { get; } = name;
    public NamedValueConvertors Convertor { get; } = convertor;
}