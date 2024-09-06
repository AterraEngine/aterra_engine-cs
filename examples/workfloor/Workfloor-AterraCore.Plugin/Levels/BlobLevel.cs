﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Attributes;
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
    protected override AssetId[] LogicSystems { get; set; } = [
        AssetIdLib.AterraLib.SystemsLogic.PlayerController,
        AssetIdLib.AterraLib.SystemsLogic.CameraController,
        WorkfloorIdLib.SystemsLogic.LevelSwitch,
        WorkfloorIdLib.SystemsLogic.RandomImpulse,
        // "Workfloor:ApplyRandomImpulseCamera",
        AssetIdLib.AterraLib.SystemsLogic.ApplyImpulse,
        AssetIdLib.AterraLib.SystemsLogic.ApplyImpulseCamera
    ];

    protected override AssetId[] RenderSystems { get; set; } = [
        AssetIdStringLib.AterraLib.SystemsRendering.Render2D,
        AssetIdLib.AterraLib.SystemsRendering.RaylibKeyHandler
    ];

    protected override AssetId[] UiSystems { get; set; } = [
    ];
}

[UsedImplicitly]
[Level(WorkfloorIdLib.Levels.Blob, CoreTags.Level)]
public class BlobLevel(
    IDirectChildren children,
    [InjectAs] BlobLevelSystemIds systemIds,
    IAssetInstanceAtlas instanceAtlas
) : NexitiesEntity(children, systemIds), INexitiesLevel {
    private IDirectChildren? _children = children;
    public IDirectChildren ChildrenIDs => _children ??= GetComponent<IDirectChildren>();

    private ISystemIds? _systemIds = systemIds;
    public ISystemIds NexitiesSystemIds => _systemIds ??= GetComponent<ISystemIds>();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override void ClearCaches() {}
    public void OnLevelFirstCreation() {
        const int entitiesPerLevel = 10_000;

        int a = (int)(Math.Sqrt(entitiesPerLevel) / 2f);
        Parallel.For((long)-a, a, body: k => {
            Parallel.For((long)-a, a, body: j => {
                if (!instanceAtlas.TryCreate(j % 2 == 0 ? WorkfloorIdLib.Entities.DuckyHype : WorkfloorIdLib.Entities.DuckyPlatinum, out IActor2D? newDucky)) return;
                newDucky.Transform2D.Translation = new Vector2(j, k);
                newDucky.Transform2D.Scale = Vector2.One;
                if (!ChildrenIDs.TryAdd(newDucky.InstanceId)) throw new ApplicationException("Entity could not be added");
            });
        });

        if (!instanceAtlas.TryCreate(AssetIdStringLib.AterraLib.Entities.Camera2D, out ICamera2D? camera2D)) return;
        camera2D.RaylibCamera2D.Camera = camera2D.RaylibCamera2D.Camera with {
            Target = new Vector2(0, 0),
            Offset = new Vector2(Raylib.GetScreenWidth() / 2f, Raylib.GetScreenHeight() / 2f),
            Rotation = 0,
            Zoom = 10
        };
        ChildrenIDs.TryAddFirst(camera2D.InstanceId);
    }
}