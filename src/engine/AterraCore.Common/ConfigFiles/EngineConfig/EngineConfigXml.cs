// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.ConfigFiles.EngineConfig.Elements;
using CodeOfChaos.Extensions;
using CodeOfChaos.Extensions.Serilog;
using Serilog;
using System.Xml.Serialization;
using static System.Environment;

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
            .Append("Engine PluginDtos loaded with the following data:")
            .AppendLineValued("- Engine version: ", Engine.Version)
            .AppendLineValued("- Game: ", Game.Version)
            .AppendLineValued("- Plugin RootFolder: ", destruct:true, LoadOrder.RootFolderRelative)
            
            .Append($"{NewLine}Plugins - Load Order : (Ids are not final)");

        int offset = 0;
        if (LoadOrder.RootAssembly is not null) {
            valuedBuilder.AppendLineValued($"- id_{offset++} : ", $"ROOT-ASSEMBLY => {LoadOrder.RootAssembly.NameSpace}");
        }
        
        LoadOrder.Plugins
            .Select(r => (r.FilePath, Id: offset++))
            .IterateOver(box => valuedBuilder.AppendLineValued($"- id_{box.Id} : ", box.FilePath));
        
        logger.Information(
            valuedBuilder.ToString(), // Message template 
            valuedBuilder.ValuesToArray() // array of all parameters to insert
        );
    }
}
