// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Common.Types;
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
}
