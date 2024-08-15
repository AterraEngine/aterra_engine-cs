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
[System(AssetIdLib.AterraCore.SystemsLogic.ApplyImpluse, CoreTags.LogicSystem)]
[Injectable<ApplyImpulse>(ServiceLifetime.Singleton)]
[UsedImplicitly]
public class ApplyImpulse() : NexitiesSystem<IActor2D>(uncached:true) {
    protected override Predicate<IActor2D> Filter { get; } = entity => !entity.Impulse2D.IsEmpty;
    
    public override void Tick(IAterraCoreWorld world) {
        IActor2D[] entities = GetEntities(world);
        foreach (IActor2D entity in entities) {
            entity.Transform2D.Translation += entity.Impulse2D.TranslationOffset;
            entity.Transform2D.Scale += entity.Impulse2D.ScaleOffset;
            entity.Transform2D.Rotation += entity.Impulse2D.RotationOffset;

            entity.Impulse2D.Clear();
        }
    }
}
