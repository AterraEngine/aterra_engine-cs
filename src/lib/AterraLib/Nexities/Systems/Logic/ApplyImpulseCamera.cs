// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.OmniVault.World;
using Microsoft.Extensions.DependencyInjection;

namespace AterraLib.Nexities.Systems.Logic;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[System(AssetIdLib.AterraCore.SystemsLogic.ApplyImpulseCamera, CoreTags.LogicSystem)]
[Injectable<ApplyImpulseCamera>(ServiceLifetime.Singleton)]
[UsedImplicitly]
public class ApplyImpulseCamera : NexitiesSystemWithFilter<ICamera2D> {
    protected override Predicate<ICamera2D> Filter { get; } = entity => !entity.Impulse2D.IsEmpty;

    public override void Tick(ActiveLevel level) {
        foreach (ICamera2D entity in GetEntities(level)) {
            
            entity.RaylibCamera2D.Camera = entity.RaylibCamera2D.Camera with {
                Target = entity.RaylibCamera2D.Camera.Target + entity.Impulse2D.TranslationOffset,
                Rotation = entity.RaylibCamera2D.Camera.Rotation + entity.Impulse2D.RotationOffset,
                Zoom = entity.RaylibCamera2D.Camera.Zoom * entity.Impulse2D.ScaleOffset.X,
                Offset = entity.RaylibCamera2D.Camera.Offset
            };
            
            entity.Impulse2D.Clear();
        }
    }
}
