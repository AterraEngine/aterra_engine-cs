// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace ArgsParser;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Represents an attribute used for specifying an argument value.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public abstract class ArgValueAttribute(char shortName, string longName, string? description = null) 
    : ArgsParserAttribute(shortName, longName, description);