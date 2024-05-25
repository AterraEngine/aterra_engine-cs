// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

namespace AterraCore.Nexities.Assets.InstanceDto.Elements;

using AterraCore.Common.FlexiPlug;
using System.Xml.Serialization;

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