// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Data;
using AterraCore.Common.Types;
using System.Xml.Serialization;
using Xml.Elements;

namespace AterraCore.Common.ConfigFiles.EngineConfig.Elements;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class LoadOrderDto {
    [XmlAttribute("relativeRootPath")] public string RootFolderRelative { get; set; } = Paths.Plugins.Folder;
    [XmlAttribute("aterraLib")] public string VersionValue { get; set; } = SemanticVersion.Zero;  // Implicit converted
    [XmlIgnore] private SemanticVersion? _versionCache;
    [XmlIgnore] public SemanticVersion AterraLibVersion => _versionCache ??= VersionValue;
    
    [XmlElement("plugin")] public FileDto[] Plugins { get; set; } = [];
}
