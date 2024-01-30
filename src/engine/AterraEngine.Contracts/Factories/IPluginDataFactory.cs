// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Core.Types;

namespace AterraEngine.Contracts.Factories;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IPluginDataFactory {
    PluginId PluginId { get;}
    void Define(PluginId pluginId);
    
    void CreateData();
    
    public int LazyNextInternalId();
    public EngineAssetId NewEngineAssetId();
    public EngineAssetId NewEngineAssetId(int value);
}