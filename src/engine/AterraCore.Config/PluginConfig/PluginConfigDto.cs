// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Xml.Serialization;
using AterraCore.Common;
using AterraCore.Config.Xml;

namespace AterraCore.Config.PluginConfig;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[XmlRoot("PluginConfig")]
public class PluginConfigDto {
    [XmlElement("EngineVersion")] 
    public required SemanticVersionDto GameVersion { get; set; }
    
    [XmlArray("Dlls")]
    [XmlArrayItem("Dll", typeof(string))] 
    public required string[] Dlls { get; set; }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public static PluginConfigDto CreateEmptyConfigDto() => 
        new() {
            GameVersion = SemanticVersion.Zero,
            Dlls = []
        };
}