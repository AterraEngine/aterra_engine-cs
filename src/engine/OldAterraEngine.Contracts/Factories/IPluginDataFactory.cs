// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using OldAterraEngine.Core.Types;

namespace OldAterraEngine.Contracts.Factories;

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