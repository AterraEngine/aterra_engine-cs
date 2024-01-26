// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Contracts.EngineFactory.Config;

namespace AterraEngine.Contracts.Engine;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IEngine {
    public EngineConfigDto EngineConfig { get; }
    
    public void ConfigureFromLoader(EngineConfigDto engineConfig);
    public void Run();
}