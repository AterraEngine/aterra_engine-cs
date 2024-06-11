// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Nexities.DataParsing.NamedValues;

namespace AterraCore.Nexities.Parsers.NamedValues.PreMade;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class NamedValueVector3Attribute(string? name = null) : NamedValueAttribute(name, NamedValueConvertors.ToVector3);
