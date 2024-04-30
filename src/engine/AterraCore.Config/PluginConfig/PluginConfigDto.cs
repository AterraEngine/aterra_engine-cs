// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Xml.Serialization;
using AterraCore.Common;
using AterraCore.Config.Xml;
using AterraCore.Contracts.Config;
using AterraCore.Contracts.Config.PluginConfig;
using AterraCore.Contracts.Config.Xml;

namespace AterraCore.Config.PluginConfig;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[XmlRoot("PluginConfig")]
public class PluginConfigDto : IConfigDto<PluginConfigDto>, IPluginConfigDto {
    [XmlElement("Name")] 
    public string ReadableName { get; set; } = null!;

    [XmlElement("Author")]
    public string Author { get; set; } = string.Empty;

    [XmlElement("PluginVersion")]
    public SemanticVersion PluginVersion { get; set; }
    
    [XmlElement("ExpectedGameVersion")]
    public SemanticVersion GameVersion { get; set; }

    [XmlArray("Bins")]
    [XmlArrayItem("Bin", typeof(FileDto))]
    public FileDto[] BinDtos { get; set; } = []; 
    [XmlIgnore] public IEnumerable<IFileDto> Dlls => BinDtos;

    // TODO add requirements? (either other plugins, or specific DLL's?)
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public PluginConfigDto PopulateAsEmpty() {
        ReadableName = string.Empty;
        Author = "Unknown";
        PluginVersion = SemanticVersion.Zero;
        GameVersion = SemanticVersion.Zero;
        BinDtos = [];

        return this;
    }
}