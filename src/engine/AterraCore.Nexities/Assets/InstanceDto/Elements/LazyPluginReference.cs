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
    [XmlAttribute("readableName")]
    public required string Name { get; set; }

    [XmlAttribute("refId")] 
    public required string InternalRef { get; set; }
    
    private PluginId? _referencedPluginId;
    public PluginId ReferencedPluginId => _referencedPluginId ??= new PluginId(InternalRef);
}