// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types;
using System.Xml.Serialization;

namespace AterraCore.Common.ConfigFiles.EngineConfig.Elements;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class GameDto {
    [XmlAttribute("version")] public string VersionValue { get; set; } = string.Empty;
    [XmlIgnore] private SemanticVersion? _versionCache;
    [XmlIgnore] public SemanticVersion Version =>_versionCache ??= new SemanticVersion(VersionValue);
}
