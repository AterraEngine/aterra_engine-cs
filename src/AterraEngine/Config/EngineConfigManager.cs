// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine_lib.Config;
using AterraEngine_lib.structs;
using AterraEngine_lib.XmlElements;
using AterraEngine.Interfaces.Config;

namespace AterraEngine.Config;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class EngineConfigManager(string filePath):IEngineConfigManager {
    public string FilePath { get; } = filePath;
    private readonly EngineConfigParser<EngineConfig> _configParser = new();
    
    public EngineConfig GetDefaultConfig() {
        return new EngineConfig {
            Version = new SemanticVersion(0, 0, 0),
            Plugins = [], 
            RaylibConfig = new RaylibConfig { 
                Window = new RaylibWindowElement {
                    Screen = new DimensionElement {
                        Height = 100, 
                        Width = 100
                    },
                    Icon = string.Empty,
                    Title = string.Empty
                }
            }
        };
    }
    
    public bool TryLoadConfigFile(out EngineConfig engineConfig, out string? errorString) {
        // Actually load from file
        if (!_configParser.TryDeserializeFromFile(FilePath, out EngineConfig? config) && config != null) {
            engineConfig = GetDefaultConfig(); // ALWAYS RETURN A DEFAULT!
            errorString = "File coule not be parsed";
            return false;
        }
        
        errorString = null;
        engineConfig = config!; // can suppress null here, because is handled during loading 
        return true;
    }

    public bool TrySaveConfig(EngineConfig config, out string? errorString) => TrySaveConfig(config, out errorString, FilePath);
    public bool TrySaveConfig(EngineConfig config, out string? errorString, string outputPath) {
        // Serialize the config object to XML
        if (!_configParser.TrySerializeToFile(config, outputPath)) {
            errorString = "Failed to save config file";
            return false;
        }
        
        errorString = null;
        return true;
    }

}