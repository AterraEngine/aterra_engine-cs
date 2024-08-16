// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.OmniVault.World;
using AterraCore.FlexiPlug.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace AterraLib.Nexities.Systems.Logic;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[System(AssetIdLib.AterraCore.SystemsLogic.ApplyImpulse, CoreTags.LogicSystem)]
[Injectable<ApplyImpulse>(ServiceLifetime.Singleton)]
[UsedImplicitly]
public class ApplyImpulse : NexitiesSystem<IActor2D> {
    protected override Predicate<IActor2D> Filter { get; } = entity => !entity.Impulse2D.IsEmpty;
    protected override bool Uncached => true;

    public override void Tick(IAterraCoreWorld world) {
        foreach (IActor2D entity in GetEntities(world)) {
            entity.Transform2D.Translation += entity.Impulse2D.TranslationOffset;
            entity.Transform2D.Scale += entity.Impulse2D.ScaleOffset;
            entity.Transform2D.Rotation += entity.Impulse2D.RotationOffset;

            entity.Impulse2D.Clear();
        }
    }
}
