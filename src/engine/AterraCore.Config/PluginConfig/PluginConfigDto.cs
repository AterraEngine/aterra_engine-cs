// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Xml.Serialization;
using AterraCore.Common;
using AterraCore.Contracts.Config;
using AterraCore.Contracts.Config.PluginConfig;

namespace AterraCore.Config.PluginConfig;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[XmlRoot("PluginConfig")]
public class PluginConfigDto : IConfigDto<PluginConfigDto>, IPluginConfigDto {
    [XmlElement("Name")] 
    public string ReadableName { get; set; } = null!;
    
    [XmlElement("Author")] 
    public string? Author { get; set; }

    [XmlElement("PluginVersion")]
    public SemanticVersion PluginVersion { get; set; }
    
    [XmlElement("GameVersion")]
    public SemanticVersion GameVersion { get; set; }

    [XmlArray("Dlls")]
    [XmlArrayItem("Dll", typeof(string))]
    public List<string> Dlls { get; set; } = null!;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public PluginConfigDto PopulateAsEmpty() {
        ReadableName = string.Empty;
        Author = string.Empty;
        PluginVersion = SemanticVersion.Zero;
        GameVersion = SemanticVersion.Zero;
        Dlls = [];

        return this;
    }
}