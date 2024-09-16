// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Attributes;
using AterraCore.Common.Attributes.Nexities;
using AterraCore.Common.Data;
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Nexities.Components;
using AterraCore.Contracts.Nexities.Entities;
using AterraCore.Contracts.Nexities.Levels;
using AterraCore.Contracts.OmniVault.Assets;
using AterraCore.Nexities.Entities;
using AterraLib.Nexities.Components;
using JetBrains.Annotations;
using Raylib_cs;
using System.Numerics;

namespace Workfloor_AterraCore.Plugin.Levels;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Component("Workfloor:BlobLevelSystemIds")]
[UsedImplicitly]
public class BlobLevelSystemIds : SystemIds {
    protected override List<AssetId> Systems { get; } = [
        // Logic section
        AssetIdLib.AterraLib.SystemsLogic.PlayerController,
        AssetIdLib.AterraLib.SystemsLogic.CameraController,
        WorkfloorIdLib.SystemsLogic.LevelSwitch,
        WorkfloorIdLib.SystemsLogic.RandomImpulse,
        
        // "Workfloor:ApplyRandomImpulseCamera",
        AssetIdLib.AterraLib.SystemsLogic.ApplyImpulse,
        AssetIdLib.AterraLib.SystemsLogic.ApplyImpulseCamera, 
        
        // Render 2D
        AssetIdLib.AterraLib.SystemsRendering.Render2DPrepForProps,
        AssetIdLib.AterraLib.SystemsRendering.Render2DPrepForActors,
        AssetIdLib.AterraLib.SystemsRendering.Render2D,
        AssetIdLib.AterraLib.SystemsRendering.RaylibKeyHandler,
        
        // Render UI
        AssetIdLib.AterraLib.SystemsRendering.RenderUi
    ]; 
}

[UsedImplicitly]
[Level(WorkfloorIdLib.Levels.Blob, CoreTags.Level)]
public class BlobLevel(
    IDirectChildren children,
    [ResolveAsSpecific] BlobLevelSystemIds systemIds,
    IAssetInstanceAtlas instanceAtlas
) : NexitiesEntity(children, systemIds), INexitiesLevel {
    private IDirectChildren? _children = children;

    private ISystemIds? _systemIds = systemIds;
    public IDirectChildren ChildrenIDs => _children ??= GetComponent<IDirectChildren>();
    public ISystemIds NexitiesSystemIds => _systemIds ??= GetComponent<ISystemIds>();
    public void OnLevelFirstCreation() {
        const int entitiesPerLevel = 10_000;

        int a = (int)(Math.Sqrt(entitiesPerLevel) / 2f);
        Parallel.For((long)-a, a, body: _ => {
            Parallel.For((long)-a, a, body: j => {
                string entityId = j % 2 == 0 ? WorkfloorIdLib.Entities.ActorDuckyHype : WorkfloorIdLib.Entities.ActorDuckyPlatinum;
                if (!instanceAtlas.TryCreate(entityId, out IActor2D? newDucky)) throw new ApplicationException("Entity could not be created");

                newDucky.Transform2D.Translation = new Vector2(0, 0);
                newDucky.Transform2D.Scale = Vector2.One;
                if (!ChildrenIDs.TryAdd(newDucky.InstanceId)) throw new ApplicationException("Entity could not be added");
            });
        });

        if (!instanceAtlas.TryCreate(StringAssetIdLib.AterraLib.Entities.Camera2D, out ICamera2D? camera2D)) throw new ApplicationException("Entity could not be created");

        camera2D.RaylibCamera2D.Camera = camera2D.RaylibCamera2D.Camera with {
            Target = new Vector2(0, 0),
            Offset = new Vector2(Raylib.GetScreenWidth() / 2f, Raylib.GetScreenHeight() / 2f),
            Rotation = 0,
            Zoom = 10
        };

        ChildrenIDs.TryAddFirst(camera2D.InstanceId);
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override void ClearCaches() {}
}
