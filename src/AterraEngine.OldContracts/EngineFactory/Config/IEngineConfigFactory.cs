// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.DTO.EngineConfig;

namespace AterraEngine.OldContracts.EngineFactory.Config;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IEngineConfigFactory<T> where T : EngineConfigDto {
    public bool TryLoadConfigFile(string filePath, out T? engineConfig);
    public bool TrySaveConfig(T config,string outputPath);
    
}