// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Common.Types;
using CodeOfChaos.Extensions;
using CodeOfChaos.Extensions.Serilog;
using Serilog;
using System.Xml.Serialization;
using Xml.Elements;

namespace AterraCore.Common.ConfigFiles.PluginConfig;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[XmlRoot("pluginConfig")]
public class PluginConfigXml {
    [XmlElement("nameSpace")] public string NameSpace { get; set; } = null!;
    [XmlElement("nameReadable")] public string NameReadable { get; set; } = null!;
    [XmlElement("author")] public string Author { get; set; } = string.Empty;
    [XmlElement("pluginVersion")] public string PluginVersionValue { get; set; } = string.Empty;
    [XmlElement("expectedGameVersion")] public string GameVersionValue { get; set; } = string.Empty;
    [XmlArray("bins")]
    [XmlArrayItem("bin", typeof(FileDto))] public FileDto[] BinDtos { get; set; } = [];
        
    [XmlIgnore] private SemanticVersion? _pluginVersionCache;
    [XmlIgnore] public SemanticVersion PluginVersion =>_pluginVersionCache ??= new SemanticVersion(PluginVersionValue);
    [XmlIgnore] private SemanticVersion? _gameVersionCache;
    [XmlIgnore] public SemanticVersion GameVersion =>_gameVersionCache ??= new SemanticVersion(GameVersionValue);

    [XmlIgnore] public IEnumerable<FileDto> Dlls => BinDtos;
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void OutputToLog(ILogger logger) {
        ValuedStringBuilder valuedBuilder = new ValuedStringBuilder()
            .AppendLine("Plugin PluginDtos loaded with the following data:")
            .AppendLineValued("- Name: ", NameReadable)
            .AppendLineValued("- Author: ", Author)
            .AppendLineValued("- Plugin Version: ", PluginVersion)
            .AppendLineValued("- Expected Game Version: ", GameVersion)
            .AppendLine("Bins:");

        BinDtos
            .IterateOver(bin => valuedBuilder.AppendLineValued("- Bin : ", [bin.FilePath]));

        logger.Information(
            valuedBuilder.ToString(), // Message template 
            valuedBuilder.ValuesToArray() // array of all parameters to insert
        );
    }
}
