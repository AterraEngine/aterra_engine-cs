// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Text;
using System.Xml.Serialization;
using AterraCore.Common;
using AterraCore.Common.FlexiPlug;
using AterraCore.Config.Xml;
using AterraCore.Contracts.Config;
using AterraCore.Extensions;
using AterraCore.Loggers;
using Serilog;

namespace AterraCore.Config.EngineConfig;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[XmlRoot("EngineConfig")]
public class EngineConfigDto : IConfigDto<EngineConfigDto> {
    [XmlElement("EngineVersion")] 
    public SemanticVersion EngineVersion { get; set; }
    
    [XmlElement("GameVersion")]
    public SemanticVersion GameVersion { get; set; }
    
    [XmlElement("PluginData")]
    public PluginDataDto PluginData { get; set; } = null!;
    
    [XmlElement("Raylib")]
    public RaylibConfigDto RaylibConfig { get; set; } = null!;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public EngineConfigDto PopulateAsEmpty() {
        EngineVersion = SemanticVersion.Max; // Warn remove this in production
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
    
    public void OutputToLog(ILogger logger) {
        
        ValuedStringBuilder valuedBuilder = new ValuedStringBuilder()
            .AppendLine("Engine Config loaded with the following data:")
            .AppendLineValued("- Engine version: ", EngineVersion)
            .AppendLineValued("- Game: ", GameVersion)
            .AppendLineValued("- Plugin RootFolder: ", PluginData.RootFolder)
            .AppendLineValued("- Plugin Plugins: ", PluginData.Plugins)
            .AppendLineValued("- Raylib config: ", RaylibConfig)
            .AppendLine()
            
            .AppendLine("Plugins - Load Order : (Ids are not final)");
        
        PluginData.Plugins
            .Select((r, i) => new { r.FileNameInternal, Id=new PluginId(i).ToString() })
            .IterateOver(box => valuedBuilder.AppendLineValued($"- id_{box.Id} : ", box.FileNameInternal));
        
        logger.Information(valuedBuilder.ToString(), valuedBuilder.ValuesToArray());
    }
}

