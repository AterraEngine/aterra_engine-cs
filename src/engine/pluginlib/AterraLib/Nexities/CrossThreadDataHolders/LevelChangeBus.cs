// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.AssetVault;
using AterraCore.Common.Attributes.AssetVault;
using AterraCore.Contracts.Threading2.CrossData.Holders;

namespace AterraLib.Nexities.CrossThreadDataHolders;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
[CrossThreadDataHolder<ILevelChangeBus>(StringAssetIdLib.AterraCore.CrossThreadDataHolders.LevelChangeBus)]
public class LevelChangeBus : AssetInstance, ILevelChangeBus {
    private readonly object _lock = new();
    private LevelChangeState _levelChangeState = LevelChangeState.Normal;
    public LevelChangeState LevelChangeState {
        get {
            lock (_lock) return _levelChangeState;
        }
        set {
            lock (_lock) _levelChangeState = value;
        }
    }
}
