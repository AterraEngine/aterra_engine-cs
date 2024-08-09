// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Threading.Logic.EventDelegates;

namespace AterraEngine.Threading.Logic.EventDelegates;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ChangeActiveLevelEventArgs(AssetId assetId) : EventArgs, IChangeActiveLevelEventArgs {
    public AssetId NewLevelId => assetId;
}