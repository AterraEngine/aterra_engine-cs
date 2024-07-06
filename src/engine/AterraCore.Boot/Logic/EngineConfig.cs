// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.ConfigFiles.EngineConfig;
using AterraCore.Common.Data;
using AterraCore.Contracts.Boot;
using Serilog;
using Xml;

namespace AterraCore.Boot.Logic;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class EngineConfig {
    public static IEngineConfiguration ImportEngineConfig(
        this IEngineConfiguration configuration,
        string filePath,
        bool outputToLog = true
    ) {
        ILogger logger = configuration.StartupLog;
            
        XmlParser<EngineConfigXml> configXmlParser = new(
            logger,
            XmlNameSpaces.ConfigEngine, 
            Paths.Xsd.XsdEngineConfigDto
        );
        
        if (!configXmlParser.TryDeserializeFromFile(filePath, out EngineConfigXml? configDto)) {
            logger.Error("Engine config file could not be parsed");
            return configuration; //the _configDto will be null, so setter of EngineConfigDto will populate as empty
        }
        
        // setter of EngineConfig will only allow it to be set once,
        //      as long as the cache hasn't been reset to null
        configuration.EngineConfig = configDto; 
        if (outputToLog) configuration.EngineConfig.OutputToLog(logger);

        return configuration;
    }
}
