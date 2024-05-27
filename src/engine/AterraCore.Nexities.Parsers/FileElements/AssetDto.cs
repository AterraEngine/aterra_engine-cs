// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraCore.Nexities.Parsers.FileElements;

using Common.Types.Nexities;
using System.Xml.Serialization;

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
