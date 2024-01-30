// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Xml.Serialization;
using AterraEngine.Contracts.DTOs.EngineConfig.Xml;
using AterraEngine.Core.Types;

namespace AterraEngine.Contracts.DTOs.EngineConfig;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[XmlRoot("EngineConfig")]
public class EngineConfigDto {
    [XmlElement("Version")] public required SemanticVersion Version { get; set; }

    [XmlArray("Plugins")]
    [XmlArrayItem("Plugin", typeof(PluginConfigDto))]
    public required List<PluginConfigDto> Plugins { get; set; }
    
    [XmlElement("Raylib")]
    public required RaylibConfigDto RaylibConfig { get; set; }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public static EngineConfigDto CreateDefault() {
        return new EngineConfigDto {
            Version = new SemanticVersion(0, 0, 0),
            Plugins = [], 
            RaylibConfig = new RaylibConfigDto { 
                Window = new RaylibWindowElementDto {
                    Screen = new DimensionElementDto {
                        Height = 100, 
                        Width = 100
                    },
                    Icon = string.Empty,
                    Title = string.Empty
                }
            }
        };
    }
    
}

