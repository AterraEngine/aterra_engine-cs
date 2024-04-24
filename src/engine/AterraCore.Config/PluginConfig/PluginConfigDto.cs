// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Xml.Serialization;
using AterraCore.Common;
using AterraCore.Config.Xml;
using AterraCore.Contracts.Config.Xml;

namespace AterraCore.Config.PluginConfig;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[XmlRoot("PluginConfig")]
public class PluginConfigDto : IConfigDto<PluginConfigDto> {
    [XmlElement("Name")] 
    public string ReadableName { get; set; } = null!;
    
    [XmlElement("Author")] 
    public string? Author { get; set; }

    
    [XmlElement("PluginVersion")] 
    public SemanticVersionDto PluginVersion { get; set; } = null!;
    
    [XmlElement("GameVersion")] 
    public SemanticVersionDto GameVersion { get; set; } = null!;

    [XmlArray("Dlls")]
    [XmlArrayItem("Dll", typeof(string))]
    public string[] Dlls { get; set; } = null!;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public PluginConfigDto PopulateAsEmpty() {
        GameVersion = SemanticVersion.Zero;
        Dlls = [];

        return this;
    }
}