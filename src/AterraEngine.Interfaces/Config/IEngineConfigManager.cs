// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine_lib.Config;

namespace AterraEngine.Interfaces.Config;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IEngineConfigManager {
    public string FilePath { get; }
    
    public EngineConfig GetDefaultConfig();
    public bool TryLoadConfigFile(out EngineConfig engineConfig, out string? errorString);
    public bool TrySaveConfig(EngineConfig config, out string? errorString);
    public bool TrySaveConfig(EngineConfig config, out string? errorString, string outputPath);
    
}