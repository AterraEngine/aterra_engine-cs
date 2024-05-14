// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Xml.Serialization;
using AterraCore.Common;
using AterraCore.Common.FlexiPlug;
using Xml.Elements;

namespace AterraCore.Nexities.Assets.InstanceDto.Elements;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public class ComponentXmlDto {
    [XmlAttribute("assetId")]
    public required string TempAssetId { get; set; }

    private PartialAssetId? _partialAssetId;
    public PartialAssetId PartialAssetId => _partialAssetId ??= new PartialAssetId(TempAssetId.Split(":")[1]);

    private PluginId? _referencedPluginId;
    public PluginId ReferencedPluginId => _referencedPluginId ??= new PluginId(TempAssetId.Split(":")[0]);

    [XmlArray] 
    [XmlArrayItem("Value",typeof(NamedValueDto))]
    public NamedValueDto[] NamedValueDtos = [];
}