// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace AterraEngine_lib.structs;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public struct SemanticVersion : IXmlSerializable{
    public int Major { get; set; }
    public int Minor { get; set; }
    public int Patch { get; set; }

    private static readonly Regex _regex = new(@"^(\d+)\.(\d+)\.(\d+)$");

    public SemanticVersion(int major, int minor, int patch) {
        Major = major;
        Minor = minor;
        Patch = patch;
    }
    
    public SemanticVersion(string version) {
        ParseFromString(version);
    }

    private void ParseFromString(string version) {
        var match = _regex.Match(version);
        if (!match.Success) {
            throw new ArgumentException("Invalid version format. Expected format: 'Major.Minor.Patch'");
        }

        Major = int.Parse(match.Groups[1].Value);
        Minor = int.Parse(match.Groups[2].Value);
        Patch = int.Parse(match.Groups[3].Value);
        
    }
    
    public override string ToString() {
        return $"{Major}.{Minor}.{Patch}";
    }
    
    
    // XML stuff
    public XmlSchema? GetSchema() {
        return null;
    }
    
    public void ReadXml(XmlReader reader) {
        // if (reader.MoveToContent() != XmlNodeType.Element) return;
        ArgumentNullException.ThrowIfNull(reader);
        ParseFromString( reader.ReadElementContentAsString());
    }
    
    public void WriteXml(XmlWriter writer) {
        writer.WriteString(ToString());
    }
}