// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Xml.Serialization;
using AterraCore.Types;
using AterraEngine.Core.Startup.EngineConfig.Dto.Xml;

namespace AterraEngine.Core.Startup.EngineConfig.Dto;

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
    public static EngineConfigDto CreateEmpty() {
        return new EngineConfigDto {
            Version = new SemanticVersion(0, 0, 0),
            Plugins = [], 
            RaylibConfig = new RaylibConfigDto { 
                Window = new RaylibWindowElementDto {
                    Screen = new DimensionElementDto {
                        Height = 100, 
                        Width = 100
                    },
                    IconPath = string.Empty,
                    Title = string.Empty
                }
            }
        };
    }
    
}

