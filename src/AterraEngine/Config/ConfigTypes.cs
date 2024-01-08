// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Xml.Serialization;
using AterraEngine_lib.structs;

namespace AterraEngine.Config;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class PluginConfig {
    [XmlAttribute("id")]    public required string Id { get; set; }
    [XmlAttribute("name")]  public required string Name { get; set; }
    [XmlText]               public required string FilePath { get; set; }
}

[XmlRoot("EngineConfig")]
public class EngineConfig {
    [XmlElement("Version")]
    public required SemanticVersion Version { get; set; }

    [XmlArray("Plugins")]
    [XmlArrayItem("Plugin", typeof(PluginConfig))]
    public required List<PluginConfig> Plugins { get; set; } = [];
}

