// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Attributes.Nexities;
using System.Collections.Concurrent;

namespace AterraLib.Nexities.Components;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Component<ISystemIds>(StringAssetIdLib.AterraLib.Components.SystemIds)]
[UsedImplicitly]
public class SystemIds : NexitiesComponent, ISystemIds {
    protected virtual List<AssetId> Systems { get; } = [];
    public IReadOnlyList<AssetId> AllSystems => Systems;
    
    private ConcurrentBag<AssetId>? _includedSystems;
    private ConcurrentBag<AssetId> IncludedSystems => _includedSystems ??= new ConcurrentBag<AssetId>(Systems);

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    private void ClearCache() {
        _includedSystems = null;
    }
    
    public override bool Cleanup() {
        Systems.Clear();
        ClearCache();
        return true;
    }
    
    public bool TryAppendSystem(AssetId systemId) {
        if (Systems.Contains(systemId)) return false;
        Systems.Add(systemId);
        ClearCache();
        return true;
    }
}
