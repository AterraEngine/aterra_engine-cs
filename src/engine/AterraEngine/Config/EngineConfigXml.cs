// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Xml.Serialization;
using AterraCore.Common;
using AterraCore.Common.FlexiPlug;
using AterraCore.Loggers.Helpers;
using AterraEngine.Config.Elements;
using Extensions;
using Serilog;
using Xml.Contracts;
using Xml.Elements;

namespace AterraEngine.Config;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[XmlRoot("engineConfig")]
public class EngineConfigXml : IConfigDto<EngineConfigXml> {
    [XmlElement("engineVersion")] public SemanticVersion EngineVersion { get; set; }
    
    [XmlElement("gameVersion")] public SemanticVersion GameVersion { get; set; }
    
    [XmlElement("pluginData")] public PluginDataDto PluginData { get; set; } = null!;
    
    [XmlElement("raylib")] public RaylibConfigDto RaylibConfig { get; set; } = null!;

    [XmlElement("logging")] public LoggingDto Logging { get; set; } = null!;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public EngineConfigXml PopulateAsEmpty() {
        EngineVersion = SemanticVersion.Max; // Warn remove this in production
        PluginData = new PluginDataDto {
            RootFolder = Paths.Plugins.Folder,
            LoadOrder = new LoadOrderDto {
                BreakOnUnstable = false,
                Plugins = []
            }
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
        Logging = new LoggingDto {
            UseAsyncConsole = false
        };

        return this;
    }
    
    public void OutputToLog(ILogger logger) {
        
        ValuedStringBuilder valuedBuilder = new ValuedStringBuilder()
            .AppendLine("Engine Config loaded with the following data:")
            .AppendLineValued("- Engine version: ", EngineVersion)
            .AppendLineValued("- Game: ", GameVersion)
            .AppendLineValued("- Plugin RootFolder: ", PluginData.RootFolder)
            .AppendLineValued("- Plugin Plugins: ", PluginData.LoadOrder.Plugins.ToList())
            .AppendLineValued("- Raylib config: ", RaylibConfig)
            .AppendLine()
            
            .AppendLine("Plugins - Load Order : (Ids are not final)");
        
        PluginData.LoadOrder.Plugins
            .Select((r, i) => new { r.FileNameInternal, Id=new PluginId(i).ToString() })
            .IterateOver(box => valuedBuilder.AppendLineValued($"- id_{box.Id} : ", box.FileNameInternal));
        
        logger.Information(valuedBuilder.ToString(), valuedBuilder.ValuesToArray());
    }
}

