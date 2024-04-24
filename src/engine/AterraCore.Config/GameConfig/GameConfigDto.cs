// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Xml.Serialization;
using AterraCore.Common;
using AterraCore.Config.Xml;
using AterraCore.Contracts.Config.Xml;

namespace AterraCore.Config.GameConfig;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

[XmlRoot("EngineConfig")]
public class GameConfigDto : IConfigDto<GameConfigDto> {
    [XmlElement("Version")] 
    public SemanticVersionDto GameVersion { get; set; } = null!;
    public SemanticVersionDto EngineVersion { get; set; } = null!;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public GameConfigDto PopulateAsEmpty() {
        GameVersion = SemanticVersion.Zero;
        EngineVersion = SemanticVersion.Zero;
        return this;
    }
}