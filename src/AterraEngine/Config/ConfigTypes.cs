// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Xml.Serialization;

using AterraEngine_lib.structs;

namespace AterraEngine.Config;

// ---------------------------------------------------------------------------------------------------------------------
// Config Elements
// ---------------------------------------------------------------------------------------------------------------------
public class PluginConfig {
    [XmlAttribute("id")]            public required string XmlId { get => Id.ToString(); set => Id = new PluginId(value); }           
    [XmlIgnore]                     public required PluginId Id { get; set; }
    [XmlAttribute("nameInternal")]  public required string NameInternal { get; set; }
    [XmlAttribute("nameReadable")]  public required string NameReadable { get; set; }
    [XmlText]                       public required string FilePath { get; set; }
}

// ---------------------------------------------------------------------------------------------------------------------
// Actual Config Class
// ---------------------------------------------------------------------------------------------------------------------
[XmlRoot("EngineConfig")]
public class EngineConfig {
    [XmlElement("Version")]
    public required SemanticVersion Version { get; set; }

    [XmlArray("Plugins")]
    [XmlArrayItem("Plugin", typeof(PluginConfig))]
    public required List<PluginConfig> Plugins { get; set; } = [];
    
}

