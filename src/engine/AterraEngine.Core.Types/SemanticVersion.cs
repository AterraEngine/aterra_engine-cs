// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace AterraEngine.Core.Types;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Represents a semantic version number.
/// </summary>
/// <remarks>
/// A semantic version number consists of three components: Major, Minor, and Patch.
/// The Major version number is incremented when incompatible changes are made.
/// The Minor version number is incremented when new, backwards-compatible features are added.
/// The Patch version number is incremented when backwards-compatible fixes are made.
/// </remarks>
public struct SemanticVersion : IXmlSerializable{
    public int Major { get; set; }
    public int Minor { get; set; }
    public int Patch { get; set; }

    private static readonly Regex _regex = new(@"^(\d+)\.(\d+)\.(\d+)$");
    
    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new instance of the SemanticVersion class with the specified version numbers.
    /// </summary>
    /// <param name="major">The major version number.</param>
    /// <param name="minor">The minor version number.</param>
    /// <param name="patch">The patch version number.</param>
    public SemanticVersion(int major, int minor, int patch) {
        Major = major;
        Minor = minor;
        Patch = patch;
    }

    /// <summary>
    /// Initializes a new instance of the SemanticVersion class using the specified version string.
    /// </summary>
    /// <param name="version">A string representing the version number.</param>
    public SemanticVersion(string version) {
        ParseFromString(version);
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Parses the version string and sets the major, minor, and patch numbers.
    /// </summary>
    /// <param name="version">The version string in the format 'Major.Minor.Patch'.</param>
    /// <exception cref="ArgumentException">Thrown when the version string is in an invalid format.</exception>
    private void ParseFromString(string version) {
        Match match = _regex.Match(version);
        if (!match.Success) {
            throw new ArgumentException("Invalid version format. Expected format: 'Major.Minor.Patch'");
        }

        Major = int.Parse(match.Groups[1].Value);
        Minor = int.Parse(match.Groups[2].Value);
        Patch = int.Parse(match.Groups[3].Value);
        
    }

    /// <summary>
    /// Returns a string that represents the current object.
    /// </summary>
    /// <returns>
    /// A string that represents the current object. The string format is "{Major}.{Minor}.{Patch}".
    /// </returns>
    public override string ToString() {
        return $"{Major}.{Minor}.{Patch}";
    }
    
    // -----------------------------------------------------------------------------------------------------------------
    // XML Parser elements
    // -----------------------------------------------------------------------------------------------------------------
    public XmlSchema? GetSchema() {
        return null;
    }

    public void ReadXml(XmlReader reader) {
        ArgumentNullException.ThrowIfNull(reader);
        ParseFromString( reader.ReadElementContentAsString());
    }

    public void WriteXml(XmlWriter writer) {
        writer.WriteString(ToString());
    }
}