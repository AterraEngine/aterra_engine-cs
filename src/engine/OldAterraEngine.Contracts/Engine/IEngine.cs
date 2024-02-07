// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using OldAterraEngine.Contracts.DTOs.EngineConfig;

namespace OldAterraEngine.Contracts.Engine;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IEngine {
    public EngineConfigDto EngineConfig { get; }
    
    public void ConfigureFromLoader(EngineConfigDto engineConfig);
    public void Run();
}