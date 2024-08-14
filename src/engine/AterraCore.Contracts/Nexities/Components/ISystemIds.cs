// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;

namespace AterraCore.Contracts.Nexities.Components;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ISystemIds : INexitiesComponent {
    public IReadOnlyCollection<AssetId> LogicSystemIds { get; } 
    public IReadOnlyCollection<AssetId> RenderSystemIds { get; }
}
