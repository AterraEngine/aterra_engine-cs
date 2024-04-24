﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Xml.Serialization;
using AterraCore.Config.Xml;

namespace AterraCore.Config.GameConfig;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

[XmlRoot("EngineConfig")]
public class GameConfigDto {
    [XmlElement("Version")] 
    public required SemanticVersionDto Version { get; set; }
}