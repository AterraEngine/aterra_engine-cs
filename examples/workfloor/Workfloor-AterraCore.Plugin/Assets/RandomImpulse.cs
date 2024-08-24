// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Attributes;
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Nexities.Entities;
using AterraCore.Contracts.OmniVault.World;
using AterraCore.Nexities.Systems;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using System.Numerics;

namespace Workfloor_AterraCore.Plugin.Assets;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[System("Workfloor:ApplyRandomImpulse", CoreTags.LogicSystem)]
[Injectable<RandomImpulse>(ServiceLifetime.Singleton)]
[UsedImplicitly]
public class RandomImpulse : NexitiesSystemWithFilter<IActor2D> {
    protected override Predicate<IActor2D> Filter { get; } = entity => entity is not IPlayer2D;
    private readonly Random _random  = new();
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override void Tick(ActiveLevel level) {
        foreach (IActor2D entity in GetEntities(level)) {
            entity.Impulse2D.TranslationOffset += new Vector2(
                (2 * _random.NextSingle() - 1) * 10f,
                (2 * _random.NextSingle() - 1) * 10f
            ) ;
            entity.Impulse2D.ScaleOffset += new Vector2(
                (2 * _random.NextSingle() - 1) * 10f,
                (2 * _random.NextSingle() - 1) * 10f
            ) ;

            entity.Impulse2D.RotationOffset += 2 * _random.NextSingle() - 1;
        }
    }
}
[System("Workfloor:ApplyRandomImpulseCamera", CoreTags.LogicSystem)]
[Injectable<RandomImpulseCamera>(ServiceLifetime.Singleton)]
[UsedImplicitly]
public class RandomImpulseCamera : NexitiesSystem<ICamera2D> {
    // -----------------------------------------------------------------------------------------------------------------
    public override void Tick(ActiveLevel level) {
        foreach (ICamera2D entity in GetEntities(level)) {
            entity.Impulse2D.RotationOffset += -0.1f;
        }
    }
}

