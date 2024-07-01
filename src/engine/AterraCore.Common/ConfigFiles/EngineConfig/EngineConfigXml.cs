// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.ConfigFiles.EngineConfig.Elements;
using AterraCore.Common.Types.FlexiPlug;
using CodeOfChaos.Extensions;
using CodeOfChaos.Extensions.Serilog;
using Serilog;
using System.Xml.Serialization;

namespace AterraCore.Common.ConfigFiles.EngineConfig;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[XmlRoot("engineConfig")]
public class EngineConfigXml {
    [XmlElement("engine")] public EngineDto Engine { get; set; } = new();
    [XmlElement("game")] public GameDto Game { get; set; } = new();
    [XmlElement("boot")] public BootConfigDto BootConfig { get; set; } = new();
    [XmlElement("loadOrder")] public LoadOrderDto LoadOrder { get; set; } = new();
    [XmlElement("raylib")] public RaylibConfigDto RaylibConfig { get; set; } = new();
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void OutputToLog(ILogger logger) {

        ValuedStringBuilder valuedBuilder = new ValuedStringBuilder()
            .AppendLine("Engine PluginDtos loaded with the following data:")
            .AppendLineValued("- Engine version: ", Engine.Version)
            .AppendLineValued("- Game: ", Game.Version)
            .AppendLineValued("- Plugin RootFolder: ",destruct:true, LoadOrder.RootFolderRelative)
            .AppendLine()
            .AppendLine("Plugins - Load Order : (Ids are not final)");

        int offset = 0;
        if (LoadOrder.RootAssembly is not null) {
            valuedBuilder.AppendLineValued($"- id_{offset++} : ", $"ROOT-ASSEMBLY => {LoadOrder.RootAssembly.NameSpace}");
        }
        
        LoadOrder.Plugins
            .Select((r, i) => new { r.FilePath, Id = offset})
            .IterateOver(box => valuedBuilder.AppendLineValued($"- id_{box.Id} : ", box.FilePath));

        valuedBuilder.AppendLine();

        logger.Information(valuedBuilder.ToString(), valuedBuilder.ValuesToArray());
    }
}
