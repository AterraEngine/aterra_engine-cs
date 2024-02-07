// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using OldAterraEngine.Core.Types;

namespace OldAterraEngine.Contracts.Factories;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class APluginDataFactory: IPluginDataFactory {
    public PluginId PluginId { get; private set; }
    private int _internalIdCounter;
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods for Plugin Factory to Use
    // -----------------------------------------------------------------------------------------------------------------
    public void Define(PluginId pluginId) {
        PluginId = pluginId;
    }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public int LazyNextInternalId() => _internalIdCounter++;
    public EngineAssetId NewEngineAssetId() => new(PluginId, LazyNextInternalId());
    public EngineAssetId NewEngineAssetId(int value) => new(PluginId, value);
    
    public virtual void CreateData() {}
}