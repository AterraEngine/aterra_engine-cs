// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Interfaces.Config;

namespace AterraEngine.EngineLoader.Config;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class EngineConfigManager<T>:IEngineConfigManager<T> where T : IEngineConfig {
    private readonly EngineConfigParser<T> _configParser = new();
    
    public T LoadConfigFile(string filePath) {
        // Actually load from file
        _configParser.TryDeserializeFromFile(filePath, out T? engineConfig);
        return engineConfig ?? (T)T.CreateDefault() ;
    }

    public bool TrySaveConfig(T config, string outputPath) {
        // Serialize the config object to XML
        return _configParser.TrySerializeToFile(config, outputPath);
    }

}