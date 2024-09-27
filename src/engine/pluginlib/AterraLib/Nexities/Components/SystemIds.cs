// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Attributes.Nexities;

namespace AterraLib.Nexities.Components;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Component<ISystemIds>(StringAssetIdLib.AterraLib.Components.SystemIds)]
[UsedImplicitly]
public class SystemIds : NexitiesComponent, ISystemIds {
    private IReadOnlyCollection<AssetId>? _logicSystemIdsCache;
    private IReadOnlyCollection<AssetId>? _renderSystemIdsCache;
    private IReadOnlyCollection<AssetId>? _uiSystemIdsCache;
    protected virtual AssetId[] LogicSystems { get; set; } = [];

    protected virtual AssetId[] RenderSystems { get; set; } = [];

    protected virtual AssetId[] UiSystems { get; set; } = [];
    public IReadOnlyCollection<AssetId> LogicSystemIds => _logicSystemIdsCache ?? LogicSystems.AsReadOnly();
    public IReadOnlyCollection<AssetId> RenderSystemIds => _renderSystemIdsCache ?? RenderSystems.AsReadOnly();
    public IReadOnlyCollection<AssetId> UiSystemIds => _uiSystemIdsCache ?? UiSystems.AsReadOnly();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void AppendLogicSystem(AssetId logicSystem) {
        LogicSystems = LogicSystems.Append(logicSystem).ToArray();
        _logicSystemIdsCache = null;
    }

    public void AppendRenderSystem(AssetId renderSystem) {
        RenderSystems = RenderSystems.Append(renderSystem).ToArray();
        _renderSystemIdsCache = null;
    }

    public void AppendUiSystem(AssetId uiSystem) {
        UiSystems = UiSystems.Append(uiSystem).ToArray();
        _uiSystemIdsCache = null;
    }
}
