﻿// ---------------------------------------------------------------------------------------------------------------------
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
using JetBrains.Annotations;
using Raylib_cs;
using System.Collections.Concurrent;
using System.Numerics;

namespace Workfloor_AterraCore.Plugin.Levels.Fractals;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
[Level(WorkfloorIdLib.Levels.FractalDragon, CoreTags.Level)]
public class DragonDucksLevel(
    IDirectChildren children,
    [ResolveAsSpecific("01J5RA7EDMS1PRR1BMRN9XM9AA")] MainLevelSystemIds systemIds,
    IAssetInstanceAtlas instanceAtlas
) : NexitiesEntity(children, systemIds), INexitiesLevel {
    private IDirectChildren? _children = children;

    private ISystemIds? _systemIds = systemIds;
    public IDirectChildren ChildrenIDs => _children ??= GetComponent<IDirectChildren>();
    public ISystemIds NexitiesSystemIds => _systemIds ??= GetComponent<ISystemIds>();
    public override void OnCreate(Ulid instanceId, AssetId assetId) {
        base.OnCreate(instanceId, assetId);

        const int iterations = 16;
        var startVector = new Vector2(-100, 0);
        var endVector = new Vector2(100, 0);

        var entityPool = new ConcurrentStack<IProp2D>();
        Parallel.For(0, 100_000, body: _ => {
            if (!instanceAtlas.TryCreate(WorkfloorIdLib.Entities.PropDuckyPlatinum, out IProp2D? newDucky)) return;

            entityPool.Push(newDucky);
        });

        var operationQueue = new Queue<(Vector2, Vector2, int)>();
        operationQueue.Enqueue((startVector, endVector, iterations));

        while (operationQueue.TryDequeue(out (Vector2, Vector2, int) tuple)) {
            (Vector2 start, Vector2 end, int depth) = tuple;

            if (depth == 0) {
                if (!entityPool.TryPop(out IProp2D? entity) || !instanceAtlas.TryCreate(WorkfloorIdLib.Entities.PropDuckyPlatinum, out entity)) continue;

                entity.Transform2D.Translation = (start + end) / 2;
                if (!ChildrenIDs.TryAdd(entity.InstanceId)) throw new ApplicationException("Entity could not be added");

                continue;
            }

            Vector2 mid = (start + end) / 2;
            var perp = new Vector2(mid.Y - start.Y, start.X - mid.X);// Perpendicular vector
            mid += perp;

            operationQueue.Enqueue((start, mid, depth - 1));
            operationQueue.Enqueue((mid, end, depth - 1));
        }

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
