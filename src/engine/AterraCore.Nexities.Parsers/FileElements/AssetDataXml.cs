// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraCore.Nexities.Parsers.FileElements;

using System.Xml.Serialization;
using Xml.Contracts;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[XmlRoot("AssetData")]
public class AssetDataXml : IXmlFileDto<AssetDataXml> {
    [XmlArray("requirements")]
    [XmlArrayItem("plugin")]
    public RequiredPluginDto[] RequiredPlugins { get; set; } = [];

    [XmlArray("assets")]
    [XmlArrayItem("asset")]
    public AssetDto[] Assets { get; set; } = [];
    
    
    // TODO add overwritable asset data so we can change data from other plugins 
    
    public AssetDataXml PopulateAsEmpty() {
        return new AssetDataXml();
    }
}
