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
    public IReadOnlyCollection<AssetId> UiSystemIds { get; }

    public void AppendLogicSystem(AssetId logicSystem);
    public void AppendRenderSystem(AssetId renderSystem);
    public void AppendUiSystem(AssetId uiSystem);
}
