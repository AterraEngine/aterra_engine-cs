// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Xml.Serialization;

using AterraEngine_lib.structs;
using AterraEngine_lib.XmlElements;

namespace AterraEngine_lib.Config;

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

public class RaylibWindowElement {
    [XmlElement("screen")]          public required DimensionElement Screen { get; set; }
    [XmlElement("icon")]            public required string Icon { get; set; }
    [XmlElement("title")]           public required string Title { get; set; }
}

public class RaylibConfig {
    [XmlElement("window")]          public required RaylibWindowElement Window { get; set; }
}

// ---------------------------------------------------------------------------------------------------------------------
// Actual Config Class
// ---------------------------------------------------------------------------------------------------------------------
[XmlRoot("EngineConfig")]
public class EngineConfig {
    [XmlElement("Version")] public required SemanticVersion Version { get; set; }

    [XmlArray("Plugins")]
    [XmlArrayItem("Plugin", typeof(PluginConfig))]
    public required List<PluginConfig> Plugins { get; set; }
    
    [XmlElement("Raylib")]
    public required RaylibConfig RaylibConfig { get; set; }
    
}

