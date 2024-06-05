// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Xml.Serialization;

namespace AterraCore.Nexities.Parsers.FileElements;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public class AssetDto {
    [XmlAttribute("assetId")]
    public string RawAssetId { get; set; } = string.Empty;

    [XmlAttribute("guid")]
    public string RawGuid { get; set; } = string.Empty;
    public Guid? Guid => System.Guid.TryParse(RawGuid, out Guid id) ? id : null;

    [XmlElement("component")]
    public ComponentDto[] Components { get; set; } = [];

    [XmlElement("assetTree")]
    public AssetTreeDto? AssetTree { get; set; }
}
