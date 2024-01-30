﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CliCommandParser;
using CliCommandParser.Attributes;

namespace AterraEngine_Workfloor.CliCommands;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class AterraEngineArgOptions : ParameterOptions {
    [ArgValue('c', "config")]  public string EngineConfig { get; set; } = "resources/engine_config-example.xml";
}