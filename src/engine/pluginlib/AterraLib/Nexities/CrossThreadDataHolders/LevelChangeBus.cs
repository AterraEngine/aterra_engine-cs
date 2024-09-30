// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.AssetVault;
using AterraCore.Common.Attributes.AssetVault;
using AterraCore.Contracts.Threading.CrossData.Holders;

namespace AterraLib.Nexities.CrossThreadDataHolders;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
[CrossThreadDataHolder<ILevelChangeBus>(StringAssetIdLib.AterraCore.CrossThreadDataHolders.LevelChangeBus)]
public class LevelChangeBus : AssetInstance, ILevelChangeBus {
    private readonly object _lock = new();
    private bool _levelChangeState;
    public bool IsLevelChangePending {
        get {
            lock (_lock) return _levelChangeState;
        }
        set {
            lock (_lock) _levelChangeState = value;
        }
    }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void NotifyLevelChange() => IsLevelChangePending = true;
}
