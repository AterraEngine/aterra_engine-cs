// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.ConfigFiles.EngineConfig.Elements;
using AterraCore.Common.Types;
using AterraCore.Common.Types.FlexiPlug;
using Extensions.Strings;
using Extensions;
using Serilog;
using System.Xml.Serialization;

namespace AterraCore.Common.ConfigFiles.EngineConfig;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[XmlRoot("engineConfig")]
public class EngineConfigXml {
    [XmlElement("engineVersion")] public SemanticVersion EngineVersion { get; set; } = SemanticVersion.Zero;
    [XmlElement("gameVersion")] public SemanticVersion GameVersion { get; set; } = SemanticVersion.Zero;
    [XmlElement("breakOnFlowException")] public bool BreakOnFlowException { get; set; } = true;
    [XmlElement("pluginData")] public PluginDataDto PluginData { get; set; } = new();
    [XmlElement("raylib")] public RaylibConfigDto RaylibConfig { get; set; } = new();
    [XmlElement("logging")] public LoggingDto Logging { get; set; } = new();
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void OutputToLog(ILogger logger) {

        ValuedStringBuilder valuedBuilder = new ValuedStringBuilder()
            .AppendLine("Engine PluginDtos loaded with the following data:")
            .AppendLineValued("- Engine version: ", EngineVersion)
            .AppendLineValued("- Game: ", GameVersion)
            .AppendLineValued("- Plugin RootFolder: ",destruct:true, PluginData.RootFolder)
            .AppendLine()
            .AppendLine("Plugins - Load Order : (Ids are not final)");

        int offset = 0;
        if (PluginData.LoadOrder.IncludeRootAssembly) {
            valuedBuilder.AppendLineValued($"- id_{new PluginId(offset++).ToString()} : ", "ROOT-ASSEMBLY");
        }
        
        PluginData.LoadOrder.Plugins
            .Select((r, i) => new { r.FilePath, Id = new PluginId(i+offset).ToString() })
            .IterateOver(box => valuedBuilder.AppendLineValued($"- id_{box.Id} : ", box.FilePath));

        valuedBuilder.AppendLine();

        logger.Information(valuedBuilder.ToString(), valuedBuilder.ValuesToArray());
    }
}
