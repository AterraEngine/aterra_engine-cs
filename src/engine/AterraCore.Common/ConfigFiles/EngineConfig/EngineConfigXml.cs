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
            .AppendLineValued("- Plugin RootFolder: ", PluginData.RootFolder)
            .AppendLineValued("- Plugin Plugins: ", PluginData.LoadOrder.Plugins.ToList())
            .AppendLineValued("- Raylib config: ", RaylibConfig)
            .AppendLine()
            .AppendLine("Plugins - Load Order : (Ids are not final)");

        PluginData.LoadOrder.Plugins
            .Select((r, i) => new { r.FilePath, Id = new PluginId(i).ToString() })
            .IterateOver(box => valuedBuilder.AppendLineValued($"- id_{box.Id} : ", box.FilePath));

        logger.Information(valuedBuilder.ToString(), valuedBuilder.ValuesToArray());
    }
}
