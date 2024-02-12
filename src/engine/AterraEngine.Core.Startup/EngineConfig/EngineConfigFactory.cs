// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;
using AterraEngine.Contracts.Core.Startup.Config;
using AterraEngine.Core.Startup.EngineConfig.Dto;
using Serilog;

namespace AterraEngine.Core.Startup.EngineConfig;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class EngineConfigFactory<T>(ILogger logger):IEngineConfigFactory<T> where T : EngineConfigDto {
    private readonly EngineConfigParser<T> _configParser = new(logger);
    
    public bool TryLoadConfigFile(string filePath, [NotNullWhen(true)] out T? engineConfig) {
        return _configParser.TryDeserializeFromFile(filePath, out engineConfig);
    }

    public bool TrySaveConfig(T config, string outputPath) {
        // Serialize the config object to XML
        return _configParser.TrySerializeToFile(config, outputPath);
    }

}