﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Common;
using AterraCore.Contracts.Config;
using Serilog;

namespace AterraCore.Config.GameConfig;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <inheritdoc/>
public class GameConfigParser<T>(ILogger logger) :
    ConfigXmlParser<T>(logger, "urn:aterra-engine:game-config", Paths.Xsd.XsdGameConfigDto)  
    where T : GameConfigDto, IConfigDto<T>, new() {
}
