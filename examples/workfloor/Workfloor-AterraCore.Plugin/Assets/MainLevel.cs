// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Data;
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Nexities.Components;
using AterraCore.Contracts.Nexities.Levels;
using AterraCore.Nexities.Components;
using AterraCore.Nexities.Entities;
using AterraCore.OmniVault.Assets.Attributes;
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
    IDirectChildren assetTree, 
    [InjectAs("01J5RA7EDMS1PRR1BMRN9XM9AA")] MainLevelSystemIds systemIds
) : EmptyLevel(assetTree, systemIds), IMainLevel; 

