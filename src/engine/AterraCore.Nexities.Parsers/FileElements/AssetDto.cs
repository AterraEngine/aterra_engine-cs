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
    public string? RawAssetId { get; set; }

    [XmlAttribute("guid")]
    private string? _guid { get; set; }
    public Guid? DefinedGuid => Guid.TryParse(_guid, out Guid id) ? id : null;

    [XmlElement("component")]
    public ComponentDto[] Components { get; set; } = [];

    [XmlElement("assetTree")]
    public AssetTreeDto? AssetTree { get; set; }
}
