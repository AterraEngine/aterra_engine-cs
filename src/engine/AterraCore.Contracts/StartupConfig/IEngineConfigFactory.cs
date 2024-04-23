// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;

namespace AterraCore.Contracts.StartupConfig;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IEngineConfigFactory<T> {
    
    public bool TryLoadConfigFile(string filePath, [NotNullWhen(true)] out T? engineConfig);
    public bool TrySaveConfig(T config,string outputPath);
    
}