// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Attributes;
using AterraCore.Common.Data;
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Nexities.Components;
using AterraCore.Contracts.Nexities.Levels;
using AterraCore.Nexities.Components;
using AterraCore.Nexities.Entities;
using AterraLib.Nexities.Components;
using AterraLib.Nexities.Entities;
using JetBrains.Annotations;

namespace Workfloor_AterraCore.Plugin.Assets;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Component("Workfloor:MainLevelSystemIds")]
[UsedImplicitly]
public class MainLevelSystemIds : SystemIds {
    protected override AssetId[] LogicSystems { get; set; } = [
        AssetIdLib.AterraCore.SystemsLogic.PlayerController,
        AssetIdLib.AterraCore.SystemsLogic.CameraController,
        // "Workfloor:ApplyRandomImpulse",
        // "Workfloor:ApplyRandomImpulseCamera",
        AssetIdLib.AterraCore.SystemsLogic.ApplyImpulse,
        AssetIdLib.AterraCore.SystemsLogic.ApplyImpulseCamera
    ];
    
    protected override AssetId[] RenderSystems { get; set; } = [
        AssetIdLib.AterraCore.SystemsRendering.Render2D
    ];
}

public interface IMainLevel : INexitiesLevel2D;

[UsedImplicitly]
[Entity("Workfloor:Levels/MainLevel", CoreTags.Level)]
public class MainLevel(
    IDirectChildren children,
    [InjectAs("01J5RA7EDMS1PRR1BMRN9XM9AA")] MainLevelSystemIds systemIds
) : NexitiesEntity(children, systemIds), INexitiesLevel2D {
    private IDirectChildren? _children = children;
    public IDirectChildren ChildrenIDs => _children ??= GetComponent<IDirectChildren>();

    private ISystemIds? _systemIds = systemIds;
    public ISystemIds NexitiesSystemIds => _systemIds ??= GetComponent<ISystemIds>();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override void ComponentOverwritten() {}
}

