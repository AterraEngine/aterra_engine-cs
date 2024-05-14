// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Xml.Serialization;

namespace AterraCore.Nexities.Assets.InstanceDto.Elements;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public class AssetXmlDto {
    [XmlAttribute("assetId")]
    public required string TempAssetId { get; set; }

    [XmlArray]
    [XmlArrayItem("Component", typeof(ComponentXmlDto))]
    public ComponentXmlDto[] ComponentXmlDtos { get; set; } = [];
}