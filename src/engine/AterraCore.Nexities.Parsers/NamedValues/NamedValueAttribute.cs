// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Nexities.DataParsing.NamedValues;

namespace AterraCore.Nexities.Parsers.NamedValues;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[AttributeUsage(AttributeTargets.Property)]
public class NamedValueAttribute(string? name = null, string convertor = NamedValueConvertors.ToString) : Attribute, INamedValueAttribute {
    public string? Name { get; } = name;
    public string Convertor { get; } = convertor;
}
