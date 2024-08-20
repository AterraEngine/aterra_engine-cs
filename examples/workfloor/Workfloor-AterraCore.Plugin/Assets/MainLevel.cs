// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
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

[Entity<INexitiesLevel2D>("Workfloor:Levels/MainLevel", CoreTags.Singleton)]
[UsedImplicitly]
public class MainLevel(
    IDirectChildren assetTree, 
    MainLevelSystemIds systemIds
) : EmptyLevel(assetTree, systemIds); 

