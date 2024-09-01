// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraLib.Nexities.Components;

namespace AterraLib.Nexities.Entities;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Component<EmptyLevelSystemIds>("AterraLib:Components/EmptyLevelSystemIds")]
[UsedImplicitly]
public class EmptyLevelSystemIds : SystemIds {
    protected override AssetId[] LogicSystems { get; set; } = [
        AssetIdLib.AterraCore.SystemsLogic.CameraController,
        // "Workfloor:ApplyRandomImpulse",
        // "Workfloor:ApplyRandomImpulseCamera",
        AssetIdLib.AterraCore.SystemsLogic.ApplyImpulseCamera
    ];

    protected override AssetId[] RenderSystems { get; set; } = [
        AssetIdLib.AterraCore.SystemsRendering.Render2D
    ];

    protected override AssetId[] UiSystems { get; set; } = [
    ];
}

[Component<DirectChildrenEmptyLevel>("AterraLib:Components/DirectChildrenEmptyLevel")]
[UsedImplicitly]
public class DirectChildrenEmptyLevel(
    [InjectAs("01J601ZRSVSA5MMEWMA7B2MV9X")] ActorAterraEngineLogo actorAterraEngineLogo,
    [InjectAs("01J6024ZFKAT3WQ1R4FKRBZ8SJ")] ICamera2D camera2D
) : DirectChildren {
    protected override List<Ulid> DirectChildIds { get; } = [
        actorAterraEngineLogo.InstanceId,
        camera2D.InstanceId
    ];
}

[Entity<INexitiesLevel>(AssetIdLib.AterraCore.Entities.EmptyLevel, CoreTags.Level)]
[UsedImplicitly]
public class EmptyLevel(
    [InjectAs("01J601YXRRSCWGQQY63E9AFD0Q")] DirectChildrenEmptyLevel children,
    [InjectAs("01J6022W9XGTXC4J1B55SQSGT5")] EmptyLevelSystemIds systemIds
) : NexitiesEntity(children, systemIds), INexitiesLevel {
    private IDirectChildren? _children = children;
    public IDirectChildren ChildrenIDs => _children ??= GetComponent<IDirectChildren>();

    private ISystemIds? _systemIds = systemIds;
    public ISystemIds NexitiesSystemIds => _systemIds ??= GetComponent<ISystemIds>();


    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override void ClearCaches() {}
    public void OnLevelFirstCreation() {}
}
