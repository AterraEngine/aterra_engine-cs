// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Xml.Serialization;
using AterraCore.Common;

namespace AterraCore.Config.Xml;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public class SemanticVersionDto {
    [XmlAttribute] public int Major { get; set; }
    [XmlAttribute] public int Minor { get; set; }
    [XmlAttribute] public int Patch { get; set; }

    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    // Conversion methods, allowing the DTO to be easily converted from and to a SemanticVersion instance.

    public static implicit operator SemanticVersionDto(SemanticVersion semanticVersion) => 
        new() { Major = semanticVersion.Major, Minor = semanticVersion.Minor, Patch = semanticVersion.Patch };

    public static implicit operator SemanticVersion(SemanticVersionDto semanticVersionDto) => 
        new(semanticVersionDto.Major, semanticVersionDto.Minor, semanticVersionDto.Patch);
}