// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Xml.Serialization;

using AterraEngine.Contracts.EngineFactory.Config.Xml;
using AterraEngine.Types;

namespace AterraEngine.Contracts.EngineFactory.Config;

// ---------------------------------------------------------------------------------------------------------------------
// Config Elements
// ---------------------------------------------------------------------------------------------------------------------
public class PluginConfig {
    [XmlAttribute("nameInternal")]  public required string NameInternal { get; set; }
    [XmlAttribute("nameReadable")]  public required string NameReadable { get; set; }
    [XmlText]                       public required string FilePath { get; set; }
}

public class RaylibWindowElement {
    [XmlElement("screen")]          public required DimensionElementDto Screen { get; set; }
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
public class EngineConfigDto {
    [XmlElement("Version")] public required SemanticVersion Version { get; set; }

    [XmlArray("Plugins")]
    [XmlArrayItem("Plugin", typeof(PluginConfig))]
    public required List<PluginConfig> Plugins { get; set; }
    
    [XmlElement("Raylib")]
    public required RaylibConfig RaylibConfig { get; set; }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public static EngineConfigDto CreateDefault() {
        return new EngineConfigDto {
            Version = new SemanticVersion(0, 0, 0),
            Plugins = [], 
            RaylibConfig = new RaylibConfig { 
                Window = new RaylibWindowElement {
                    Screen = new DimensionElementDto {
                        Height = 100, 
                        Width = 100
                    },
                    Icon = string.Empty,
                    Title = string.Empty
                }
            }
        };
    }
    
}

