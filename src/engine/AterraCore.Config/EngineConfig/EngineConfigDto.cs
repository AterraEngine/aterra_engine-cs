// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Xml.Serialization;
using AterraCore.Common;
using AterraCore.Common.FlexiPlug;
using AterraCore.Config.Xml;
using AterraCore.Contracts.Config;
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
        
        logger.Verbose("- Engine Config loaded with the following data:");
        
        logger.Information("Engine version: {EngineVersion}", EngineVersion);
        logger.Information("Game version: {GameVersion}", GameVersion);
        
        logger.Information("Plugin RootFolder: {Folder}", PluginData.RootFolder);
        logger.Information("Plugin Plugins: {Folder}", PluginData.Plugins);
        logger.Information("Raylib config: {RaylibConfig}", RaylibConfig);
        
        logger.Information("Plugins - Load Order : (Ids are not final)") ;
        PluginData.Plugins
            .Select((r, i) => new {
                r.FilePath,
                Id=new PluginId(i).ToString()
            })
            .ToList()
            .ForEach((o) => logger.Information("- {id} : {FilePath}", o.Id, o.FilePath ));
    }
}

