// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraEngine.Contracts.DTOs.EngineConfig;

namespace AterraEngine.Contracts.Engine;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IEngine {
    public EngineConfigDto EngineConfig { get; }
    
    public void ConfigureFromLoader(EngineConfigDto engineConfig);
    public void Run();
}