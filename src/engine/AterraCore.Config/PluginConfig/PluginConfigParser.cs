// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Common;
using AterraCore.Config.Xml;
using Serilog;

namespace AterraCore.Config.PluginConfig;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public class PluginConfigParser<T>(ILogger logger) :
    ConfigXmlParser<T>(logger, "urn:aterra-engine:plugin-config", Paths.XsdPluginConfigDto)
    
    where T : PluginConfigDto;