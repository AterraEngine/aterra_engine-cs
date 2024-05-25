// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

namespace AterraCore.Nexities.Data.Assets.InstanceDto.Elements;

using AterraCore.Common;
using AterraCore.Common.FlexiPlug;
using System.Xml.Serialization;
using Xml.Elements;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public class ComponentXmlDto {

    private PartialAssetId? _partialAssetId;

    private PluginId? _referencedPluginId;

    [XmlArray]
    [XmlArrayItem("Value", typeof(NamedValueDto))]
    public NamedValueDto[] NamedValueDtos = [];
    [XmlAttribute("assetId")]
    public required string TempAssetId { get; set; }
    public PartialAssetId PartialAssetId => _partialAssetId ??= new PartialAssetId(TempAssetId.Split(":")[1]);
    public PluginId ReferencedPluginId => _referencedPluginId ??= new PluginId(TempAssetId.Split(":")[0]);
}