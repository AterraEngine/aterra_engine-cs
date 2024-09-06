﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Data;
using AterraCore.Common.Types;
using System.Xml.Serialization;

namespace AterraCore.Common.ConfigFiles;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[XmlRoot("pluginConfig", Namespace = XmlNameSpaces.ConfigPlugin)]
public class PluginConfigXml {
    [XmlElement("nameSpace")] public string NameSpace { get; set; } = null!;
    [XmlElement("nameReadable")] public string NameReadable { get; set; } = null!;
    [XmlElement("author")] public string Author { get; set; } = string.Empty;
    [XmlElement("pluginVersion")] public string PluginVersionValue { get; set; } = string.Empty;
    [XmlElement("expectedGameVersion")] public string GameVersionValue { get; set; } = string.Empty;
    [XmlArray("bins")]
    [XmlArrayItem("bin", typeof(BinDto))] public BinDto[] BinDtos { get; set; } = [];
    [XmlArray("resources")]
    [XmlArrayItem("res", typeof(ResourceDto))] public ResourceDto[] ResourceDtos { get; set; } = [];

    [XmlIgnore] private SemanticVersion? _pluginVersionCache;
    [XmlIgnore] public SemanticVersion PluginVersion => _pluginVersionCache ??= new SemanticVersion(PluginVersionValue);
    [XmlIgnore] private SemanticVersion? _gameVersionCache;
    [XmlIgnore] public SemanticVersion GameVersion => _gameVersionCache ??= new SemanticVersion(GameVersionValue);

    [XmlIgnore] public IEnumerable<BinDto> Dlls => BinDtos;

    // For reasons, I shall use nested classes
    // Yes these reasons are undefined
    public class BinDto {
        [XmlAttribute("file")] public required string FileName { get; set; }
    }

    public class ResourceDto {
        [XmlAttribute("externalPath")] public required string ExternalFilePath { get; set; }
        [XmlAttribute("internalPath")] public required string InternalFilePath { get; set; }
    }
}
