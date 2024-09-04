// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Attributes;
using AterraCore.Common.Data;
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
[Level(WorkfloorIdLib.Levels.FractalBarnsley, CoreTags.Level)]
public class BarnsleyFernLevel(
    IDirectChildren children,
    [InjectAs("01J5RA7EDMS1PRR1BMRN9XM9AA")] MainLevelSystemIds systemIds,
    
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
        const int iterations = 1_000_000;
        var point = new Vector2(0, 0);

        var entityPool = new ConcurrentStack<IActor2D>();
        Parallel.For(0, iterations / 100, _ => {
            if (!instanceAtlas.TryCreate(WorkfloorIdLib.Entities.DuckyPlatinum, out IActor2D? newDucky)) return;
            entityPool.Push(newDucky);
        });

        for (int i = 0; i < iterations; i++) {
            point = ApplyBarnsleyFernTransform(point);

            if (i % 100 != 0) continue;
            if (!entityPool.TryPop(out IActor2D? entity) || !instanceAtlas.TryCreate(WorkfloorIdLib.Entities.DuckyPlatinum, out entity)) return;
            entity.Transform2D.Translation = point * 10f;
            if (!ChildrenIDs.TryAdd(entity.InstanceId)) throw new ApplicationException("Entity could not be added");
        }

        if (!instanceAtlas.TryCreate(AssetIdLib.AterraLib.Entities.Camera2D, out ICamera2D? camera2D)) return;
        camera2D.RaylibCamera2D.Camera = camera2D.RaylibCamera2D.Camera with {
            Target = new Vector2(0, 0),
            Offset = new Vector2(Raylib.GetScreenWidth() / 2f, Raylib.GetScreenHeight() / 2f),
            Rotation = 0,
            Zoom = 10
        };
        ChildrenIDs.TryAddFirst(camera2D.InstanceId);
    }
    
    private static Vector2 ApplyBarnsleyFernTransform(Vector2 point) {
        double rand = new Random().NextDouble();

        return rand switch {
            < 0.01 => new Vector2(0, 0.16f * point.Y),
            < 0.86 => new Vector2(0.85f * point.X + 0.04f * point.Y, -0.04f * point.X + 0.85f * point.Y + 1.6f),
            < 0.93 => new Vector2(0.2f * point.X - 0.26f * point.Y, 0.23f * point.X + 0.22f * point.Y + 1.6f),
            _      => new Vector2(-0.15f * point.X + 0.28f * point.Y, 0.26f * point.X + 0.24f * point.Y + 0.44f)
        };
    }
}
