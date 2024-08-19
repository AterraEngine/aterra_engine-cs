// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Threading.Logic.EventDelegates;

namespace AterraEngine.Threading.Logic.EventDelegates;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record ChangeActiveLevelEventArgs(AssetId NewLevelId) : IChangeActiveLevelEventArgs;