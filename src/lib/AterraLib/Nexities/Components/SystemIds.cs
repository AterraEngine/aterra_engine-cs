// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraLib.Nexities.Components;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Component<ISystemIds>(AssetIdLib.AterraCore.Components.SystemIds)]
[UsedImplicitly]
public class SystemIds : NexitiesComponent, ISystemIds {
    protected virtual AssetId[] LogicSystems { get; set; } = [];
    private IReadOnlyCollection<AssetId>? _logicSystemIdsCache;
    public IReadOnlyCollection<AssetId> LogicSystemIds => _logicSystemIdsCache ?? LogicSystems.AsReadOnly();

    protected virtual AssetId[] RenderSystems  { get; set; } = [];
    private IReadOnlyCollection<AssetId>? _renderSystemIdsCache;
    public IReadOnlyCollection<AssetId> RenderSystemIds => _renderSystemIdsCache ?? RenderSystems.AsReadOnly();
    
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

}
