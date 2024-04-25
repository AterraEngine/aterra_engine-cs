// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Xml.Serialization;
using AterraCore.Common;
using AterraCore.Contracts.Config;

namespace AterraCore.Config.GameConfig;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

[XmlRoot("EngineConfig")]
public class GameConfigDto : IConfigDto<GameConfigDto> {
    [XmlElement("Version")] 
    public SemanticVersion GameVersion { get; set; }
    
    [XmlElement("EngineVersion")] 
    public SemanticVersion EngineVersion { get; set; }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public GameConfigDto PopulateAsEmpty() {
        GameVersion = SemanticVersion.Zero;
        EngineVersion = SemanticVersion.Zero;
        return this;
    }
}