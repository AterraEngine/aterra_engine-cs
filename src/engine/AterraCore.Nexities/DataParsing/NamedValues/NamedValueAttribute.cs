// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraCore.Nexities.DataParsing.NamedValues;

using Contracts.Nexities.DataParsing.NamedValues;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[AttributeUsage(AttributeTargets.Property)]
public class NamedValueAttribute(string? name = null, string convertor = NamedValueConvertors.ToString) : Attribute, INamedValueAttribute {
    public string? Name { get; } = name;
    public string Convertor { get; } = convertor;
}