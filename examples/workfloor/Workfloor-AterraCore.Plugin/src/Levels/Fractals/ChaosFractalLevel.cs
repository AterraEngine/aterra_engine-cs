// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Attributes;
using AterraCore.Common.Attributes.Nexities;
using AterraCore.Common.Data;
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Nexities.Components;
using AterraCore.Contracts.Nexities.Entities;
using AterraCore.Contracts.Nexities.Entities.QuickHands;
using AterraCore.Contracts.Nexities.Levels;
using AterraCore.Contracts.OmniVault.Assets;
using AterraCore.Nexities.Entities;
using JetBrains.Annotations;
using Raylib_cs;
using System.Numerics;

namespace Workfloor_AterraCore.Plugin.Levels.Fractals;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
[Level(WorkfloorIdLib.Levels.FractalChaos, CoreTags.Level)]
public class ChaosFractalLevel(
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
        const uint sierpinskiDepth = 11;
        const float sierpinskiSize = 2500f;

        var p1TopVertex = new Vector2(0, (float)(sierpinskiSize / Math.Sqrt(3f)));
        var p2BottomLeft = new Vector2(-sierpinskiSize / 2, 0);
        var p3BottomRight = new Vector2(sierpinskiSize / 2, 0);

        var operationQueue = new Queue<(Vector2, Vector2, Vector2, uint)>();
        operationQueue.Enqueue((p1TopVertex, p2BottomLeft, p3BottomRight, sierpinskiDepth));

        while (operationQueue.TryDequeue(out (Vector2, Vector2, Vector2, uint) tuple)) {
            (Vector2 p1, Vector2 p2, Vector2 p3, uint depth) = tuple;
            if (depth == 0) {
                if (!instanceAtlas.TryCreate(WorkfloorIdLib.Entities.PropDuckyPlatinum, out IHasTransform2D? newDucky)) throw new ApplicationException("Entity could not be created");

                newDucky.Transform2D.Translation = (p1 + p2 + p3) / 3;
                if (!ChildrenIDs.TryAdd(newDucky.InstanceId)) throw new ApplicationException("Entity could not be added");

                continue;
            }

            Vector2 mid12 = new Vector2((float)Math.Sin(p1.X), (float)Math.Sin(p2.Y)) * (p1 + p2);
            Vector2 mid23 = new Vector2((float)Math.Sin(p2.X), (float)Math.Sin(p3.Y)) * (p2 + p3);
            Vector2 mid31 = new Vector2((float)Math.Sin(p3.X), (float)Math.Sin(p1.Y)) * (p3 + p1);

            operationQueue.Enqueue((p1, mid12, mid31, depth - 1));
            operationQueue.Enqueue((mid12, p2, mid23, depth - 1));
            operationQueue.Enqueue((mid31, mid23, p3, depth - 1));
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
