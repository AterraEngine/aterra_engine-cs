// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraLib.Nexities.Components;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Component<ISystemIds>(AssetIdLib.AterraLib.Components.SystemIds)]
[UsedImplicitly]
public class SystemIds : NexitiesComponent, ISystemIds {
    protected virtual AssetId[] LogicSystems { get; set; } = [];
    private IReadOnlyCollection<AssetId>? _logicSystemIdsCache;
    public IReadOnlyCollection<AssetId> LogicSystemIds => _logicSystemIdsCache ?? LogicSystems.AsReadOnly();

    protected virtual AssetId[] RenderSystems { get; set; } = [];
    private IReadOnlyCollection<AssetId>? _renderSystemIdsCache;
    public IReadOnlyCollection<AssetId> RenderSystemIds => _renderSystemIdsCache ?? RenderSystems.AsReadOnly();

    protected virtual AssetId[] UiSystems { get; set; } = [];
    private IReadOnlyCollection<AssetId>? _uiSystemIdsCache;
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
