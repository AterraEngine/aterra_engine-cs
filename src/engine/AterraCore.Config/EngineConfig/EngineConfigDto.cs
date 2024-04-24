// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Xml.Serialization;
using AterraCore.Common;
using AterraCore.Config.Xml;
using AterraCore.Contracts.Config;

namespace AterraCore.Config.EngineConfig;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[XmlRoot("EngineConfig")]
public class EngineConfigDto : IConfigDto<EngineConfigDto> {
    [XmlElement("Version")] 
    public SemanticVersionDto EngineVersion { get; set; } = null!;
    
    [XmlElement("PluginData")]
    public PluginDataDto PluginData { get; set; } = null!;
    
    [XmlElement("Raylib")]
    public RaylibConfigDto RaylibConfig { get; set; } = null!;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public EngineConfigDto PopulateAsEmpty() {
        EngineVersion = SemanticVersion.Zero;
        PluginData = new PluginDataDto {
            RootFolder = Paths.Plugins.Folder,
            Plugins = []
        };
        RaylibConfig = new RaylibConfigDto {
            Window = new RaylibWindowElementDto {
                Screen = new DimensionElementDto {
                    Height = 100,
                    Width = 100
                },
                IconPath = string.Empty,
                Title = ""
            }
        };
            
        return this;
    }
}

