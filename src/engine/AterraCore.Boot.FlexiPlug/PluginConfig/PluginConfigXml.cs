// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Common.Types;
using AterraCore.Contracts.FlexiPlug.Config;
using AterraCore.Loggers.Helpers;
using Extensions;
using Serilog;
using System.Xml.Serialization;
using Xml.Contracts;
using Xml.Elements;

namespace AterraCore.Boot.FlexiPlug.PluginConfig;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[XmlRoot("pluginConfig")]
public class PluginConfigXml : IXmlFileDto<PluginConfigXml>, IPluginConfigDto {

    [XmlArray("bins")]
    [XmlArrayItem("bin", typeof(FileDto))]
    public FileDto[] BinDtos { get; set; } = [];
    [XmlElement("name")]
    public string ReadableName { get; set; } = null!;

    [XmlElement("author")]
    public string Author { get; set; } = string.Empty;

    [XmlElement("pluginVersion")]
    public SemanticVersion PluginVersion { get; set; }

    [XmlElement("expectedGameVersion")]
    public SemanticVersion GameVersion { get; set; }
    [XmlIgnore] public IEnumerable<IFileDto> Dlls => BinDtos;

    // TODO add requirements? (either other plugins, or specific DLL's?)

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public PluginConfigXml PopulateAsEmpty() {
        ReadableName = string.Empty;
        Author = "Unknown";
        PluginVersion = SemanticVersion.Zero;
        GameVersion = SemanticVersion.Zero;
        BinDtos = [];

        return this;
    }

    public void OutputToLog(ILogger logger) {

        ValuedStringBuilder valuedBuilder = new ValuedStringBuilder()
            .AppendLine("Plugin Config loaded with the following data:")
            .AppendLineValued("- Name: ", ReadableName)
            .AppendLineValued("- Author: ", Author)
            .AppendLineValued("- Plugin Version: ", PluginVersion)
            .AppendLineValued("- Expected Game Version: ", GameVersion)
            .AppendLine()
            .AppendLine("Bins:");

        BinDtos
            .IterateOver(bin => valuedBuilder.AppendLineValued("- Bin : ", bin.FileNameInternal));

        logger.Debug(valuedBuilder.ToString(), valuedBuilder.ValuesToArray());
    }
}
