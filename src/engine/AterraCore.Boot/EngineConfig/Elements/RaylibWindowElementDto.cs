// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Xml.Serialization;
using Xml.Elements;

namespace AterraCore.Boot.EngineConfig.Elements;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class RaylibWindowElementDto {
    [XmlElement("screen")] public required DimensionElementDto Screen { get; set; }
    [XmlElement("icon-path")] public required string IconPath { get; set; }
    [XmlElement("title")] public required string Title { get; set; }
}
