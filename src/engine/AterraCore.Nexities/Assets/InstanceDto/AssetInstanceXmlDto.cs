// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

namespace AterraCore.Nexities.Assets.InstanceDto;

using Elements;
using System.Xml.Serialization;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[XmlRoot("instanceData")]
public class AssetInstanceXmlDto {

    [XmlArray("pluginRefs")]
    [XmlArrayItem(typeof(LazyPluginReference))]
    public LazyPluginReference[] LazyPluginReferences { get; set; } = [];

    [XmlArray("assets")]
    [XmlArrayItem(typeof(AssetXmlDto))]
    public AssetXmlDto[] AssetXmlDtos { get; set; } = [];
}