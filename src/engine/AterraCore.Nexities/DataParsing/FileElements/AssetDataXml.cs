// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraCore.Nexities.DataParsing.FileElements;

using System.Xml.Serialization;
using Xml.Contracts;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[XmlRoot("AssetData")]
public class AssetDataXml : IConfigDto<AssetDataXml> {
    [XmlArray("requirements")]
    [XmlArrayItem("plugin")]
    public PluginDto[] Plugins { get; set; }
    
    [XmlArray("assets")]
    [XmlArrayItem("asset")]
    public AssetDto[] Assets { get; set; }
    
    public AssetDataXml PopulateAsEmpty() {
        return new AssetDataXml {
            Plugins = [],
            Assets = []
        };
    }
}
