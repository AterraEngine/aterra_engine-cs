// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using OldAterraEngine.Contracts.DTOs.EngineConfig;

namespace OldAterraEngine.Contracts.EngineFactory.Config;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IEngineConfigFactory<T> where T : EngineConfigDto {
    public bool TryLoadConfigFile(string filePath, out T? engineConfig);
    public bool TrySaveConfig(T config,string outputPath);
    
}