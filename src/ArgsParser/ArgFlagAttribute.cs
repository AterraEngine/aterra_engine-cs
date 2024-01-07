// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace ArgsParser;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// Represents an attribute that is used to define an argument flag for the ArgsParser class.
/// /
[AttributeUsage(AttributeTargets.Property)]
public class ArgFlagAttribute(char shortName, string longName, string? description = null) : ArgsParserAttribute(shortName, longName, description);