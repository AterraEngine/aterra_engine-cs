// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Xml.Serialization;
using Xml.Contracts;

namespace AterraCore.Nexities.Parsers.FileElements;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[XmlRoot("AssetData")]
public class AssetDataXml : IXmlFileDto<AssetDataXml> {
    [XmlArray("requirements")]
    [XmlArrayItem("plugin", typeof(RequiredPluginDto))]
    public RequiredPluginDto[] RequiredPlugins { get; set; } = [];

    [XmlArray("assets")]
    [XmlArrayItem("asset", typeof(AssetDto))]
    public AssetDto[] Assets { get; set; } = [];

    // TODO add overwritable asset data so we can change data from other plugins 

    public AssetDataXml PopulateAsEmpty() => new();
}
