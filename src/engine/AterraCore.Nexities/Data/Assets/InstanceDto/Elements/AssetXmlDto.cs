// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

namespace AterraCore.Nexities.Data.Assets.InstanceDto.Elements;

using System.Xml.Serialization;

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