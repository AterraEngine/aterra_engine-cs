// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Attributes.DI;
using AterraCore.Common.Attributes.Nexities;
using AterraCore.Contracts.OmniVault.World;
using Microsoft.Extensions.DependencyInjection;

namespace AterraLib.Nexities.Systems.Logic;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[System(StringAssetIdLib.AterraLib.SystemsLogic.ApplyImpulse, CoreTags.LogicThread)]
[Injectable<ApplyImpulse>(ServiceLifetime.Singleton)]
[UsedImplicitly]
public class ApplyImpulse : NexitiesSystemUnCachedWithFilter<IActor2D> {
    protected override Predicate<IActor2D> Filter { get; } = entity => !entity.Impulse2D.IsEmpty;
    public override void Tick(ActiveLevel level) {
        foreach (IActor2D entity in GetEntities(level)) {
            entity.Transform2D.Translation += entity.Impulse2D.TranslationOffset;
            entity.Transform2D.Scale *= entity.Impulse2D.ScaleOffset;
            entity.Transform2D.Rotation += entity.Impulse2D.RotationOffset;

            entity.Impulse2D.Clear();
        }
    }
}
