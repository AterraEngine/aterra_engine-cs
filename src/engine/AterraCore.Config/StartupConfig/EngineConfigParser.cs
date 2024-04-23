﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Common;
using AterraCore.Config.Xml;
using Serilog;

namespace AterraCore.Config.StartupConfig;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <inheritdoc/>
public class EngineConfigParser<T>(ILogger logger) :
    ConfigXmlParser<T>(logger, "urn:aterra-engine:engine-config", Paths.XsdEngineConfigDto);

