// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Attributes;
using AterraCore.Common.Attributes.Nexities;
using AterraLib.Nexities.Components;

namespace AterraLib.Nexities.Entities;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Component<EmptyLevelSystemIds>("AterraLib:Components/EmptyLevelSystemIds")]
[UsedImplicitly]
public class EmptyLevelSystemIds : SystemIds {
    protected override AssetId[] LogicSystems { get; set; } = [
        AssetIdLib.AterraLib.SystemsLogic.PostLogicProcessing,
    ];

    protected override AssetId[] RenderSystems { get; set; } = [
        AssetIdLib.AterraLib.SystemsRendering.Render2DPrepForProps,
        AssetIdLib.AterraLib.SystemsRendering.Render2DPrepForActors,
        AssetIdLib.AterraLib.SystemsRendering.Render2D,
        AssetIdLib.AterraLib.SystemsRendering.PostRendering,
    ];

    protected override AssetId[] UiSystems { get; set; } = [
        AssetIdLib.AterraLib.SystemsRendering.RenderUi,
    ];
}

[Component<DirectChildrenEmptyLevel>("AterraLib:Components/DirectChildrenEmptyLevel")]
[UsedImplicitly]
public class DirectChildrenEmptyLevel(
    [ResolveAsSpecific("01J601ZRSVSA5MMEWMA7B2MV9X")] ActorAterraEngineLogo actorAterraEngineLogo,
    [ResolveAsSpecific("01J6024ZFKAT3WQ1R4FKRBZ8SJ")] ICamera2D camera2D
) : DirectChildren {
    protected override List<Ulid> DirectChildIds { get; } = [
        actorAterraEngineLogo.InstanceId,
        camera2D.InstanceId
    ];
}

[Entity<INexitiesLevel>(StringAssetIdLib.AterraLib.Entities.EmptyLevel, CoreTags.Level)]
[UsedImplicitly]
public class EmptyLevel(
    [ResolveAsSpecific("01J601YXRRSCWGQQY63E9AFD0Q")] DirectChildrenEmptyLevel children,
    [ResolveAsSpecific("01J6022W9XGTXC4J1B55SQSGT5")] EmptyLevelSystemIds systemIds
) : NexitiesEntity(children, systemIds), INexitiesLevel {
    private IDirectChildren? _children = children;

    private ISystemIds? _systemIds = systemIds;
    public IDirectChildren ChildrenIDs => _children ??= GetComponent<IDirectChildren>();
    public ISystemIds NexitiesSystemIds => _systemIds ??= GetComponent<ISystemIds>();
    public void OnLevelFirstCreation() {}


    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override void ClearCaches() {}
}
