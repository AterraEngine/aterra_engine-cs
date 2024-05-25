// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraCore.Nexities.DataParsing.FileElements;

using System.Xml.Serialization;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public class AssetDto {
    [XmlAttribute("assetId")]
    public string RawAssetId { get; set; }
    
    [XmlAttribute("guid")]
    public string Guid { get; set; }
    
    [XmlElement("component")]
    public ComponentDto[] Components { get; set; }
    
    [XmlElement("assetTree")]
    public AssetTreeDto AssetTree { get; set; }
}
