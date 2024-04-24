// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Xml.Serialization;
using AterraCore.Common;
using AterraCore.Config.Xml;

namespace AterraCore.Config.EngineConfig;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[XmlRoot("EngineConfig")]
public class EngineConfigDto {
    [XmlElement("Version")] 
    public required SemanticVersionDto Version { get; set; }
    
    [XmlElement("PluginData")]
    public required PluginDataDto PluginData { get; set; }
    
    [XmlElement("Raylib")]
    public required RaylibConfigDto RaylibConfig { get; set; }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public static EngineConfigDto CreateEmptyConfigDto() =>
        new() {
            Version = new SemanticVersion(0, 0, 0),
            PluginData = new PluginDataDto {
                RootFolder = "plugins",
                Plugins = []
            }, 
            RaylibConfig = new RaylibConfigDto { 
                Window = new RaylibWindowElementDto {
                    Screen = new DimensionElementDto {
                        Height = 100, 
                        Width = 100
                    },
                    IconPath = string.Empty,
                    Title = ""
                }
            }
        };
}

