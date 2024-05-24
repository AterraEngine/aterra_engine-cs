// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Xml.Serialization;
using AterraCore.Common.FlexiPlug;
namespace AterraCore.Nexities.Assets.InstanceDto.Elements;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public class LazyPluginReference {

    private PluginId? _referencedPluginId;
    [XmlAttribute("readableName")]
    public required string Name { get; set; }

    [XmlAttribute("refId")]
    public required string InternalRef { get; set; }
    public PluginId ReferencedPluginId => _referencedPluginId ??= new PluginId(InternalRef);
}