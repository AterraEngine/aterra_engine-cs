// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
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
        "AterraLib:Nexities/Systems/ApplyImpulse",
        "AterraLib:Nexities/Systems/PlayerController"
    ];
    
    protected override AssetId[] RenderSystems { get; set; } = [
        "AterraLib:Nexities/Systems/Render2D"
    ];
}

[Entity<INexitiesLevel>("NexitiesDebug:Levels/MainLevel")]
[UsedImplicitly]
public class MainLevel(
    IDirectChildren assetTree, 
    MainLevelSystemIds systemIds
) : EmptyLevel(assetTree, systemIds); 

