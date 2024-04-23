// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Serilog;

using System.Diagnostics.CodeAnalysis;
using AterraCore.Contracts.StartupConfig;


namespace AterraCore.Config.StartupConfig;

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