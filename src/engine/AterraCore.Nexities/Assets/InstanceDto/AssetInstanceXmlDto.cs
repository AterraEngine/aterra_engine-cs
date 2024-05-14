// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Xml.Serialization;
using AterraCore.Nexities.Assets.InstanceDto.Elements;

namespace AterraCore.Nexities.Assets.InstanceDto;

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