// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Xml.Serialization;

namespace AterraEngine.DTO.EngineConfig.Xml;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class RaylibWindowElementDto {
    [XmlElement("screen")]          public required DimensionElementDto Screen { get; set; }
    [XmlElement("icon")]            public required string Icon { get; set; }
    [XmlElement("title")]           public required string Title { get; set; }
}
