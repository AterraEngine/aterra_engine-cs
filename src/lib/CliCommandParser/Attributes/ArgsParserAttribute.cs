// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace CliCommandParser.Attributes;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Represents an attribute used to specify the arguments for a parser.
/// </summary>
public abstract class ArgsParserAttribute(char shortName, string longName, string? description) : Attribute {
    /// <summary>
    /// Gets the short name of the property.
    /// </summary>
    /// <value>
    /// A character representing the short name of the property.
    /// </value>
    public char ShortName { get; } = shortName;

    /// <summary>
    /// Gets the long name of the property without hyphens.
    /// </summary>
    /// <value>
    /// The long name of the property without hyphens.
    /// </value>
    public string LongName { get; } = longName.Replace("-", "");

    /// <summary>
    /// Gets the description of the property.
    /// </summary>
    /// <value>
    /// A string representing the description of the property.
    /// </value>
    public string? Description { get; } = description;
    
}