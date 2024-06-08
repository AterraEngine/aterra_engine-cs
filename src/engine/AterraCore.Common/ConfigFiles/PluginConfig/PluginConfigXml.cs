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
    [XmlElement("name")]
    public string ReadableName { get; set; } = null!;

    [XmlElement("author")]
    public string Author { get; set; } = string.Empty;

    [XmlElement("pluginVersion")] public string PluginVersionValue { get; set; } = string.Empty;
    [XmlIgnore] private SemanticVersion? _pluginVersionCache;
    [XmlIgnore] public SemanticVersion PluginVersion =>_pluginVersionCache ??= new SemanticVersion(PluginVersionValue);

    [XmlElement("expectedGameVersion")] public string GameVersionValue { get; set; } = string.Empty;
    [XmlIgnore] private SemanticVersion? _gameVersionCache;
    [XmlIgnore] public SemanticVersion GameVersion =>_gameVersionCache ??= new SemanticVersion(GameVersionValue);
    
    [XmlIgnore] public IEnumerable<FileDto> Dlls => BinDtos;

    [XmlArray("bins")]
    [XmlArrayItem("bin", typeof(FileDto))]
    public FileDto[] BinDtos { get; set; } = [];
    
    // TODO add requirements? (either other plugins, or specific DLL's?)

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void OutputToLog(ILogger logger) {
        
        ValuedStringBuilder valuedBuilder = new ValuedStringBuilder()
            .AppendLine("Plugin PluginDtos loaded with the following data:")
            .AppendLineValued("- Name: ", ReadableName)
            .AppendLineValued("- Author: ", Author)
            .AppendLineValued("- Plugin Version: ", PluginVersion)
            .AppendLineValued("- Expected Game Version: ", GameVersion)
            .AppendLine()
            .AppendLine("Bins:");

        BinDtos
            .IterateOver(bin => valuedBuilder.AppendLineValued("- Bin : ", [bin.FilePath]));

        logger.Debug(valuedBuilder.ToString(), valuedBuilder.ValuesToArray());
    }
}
