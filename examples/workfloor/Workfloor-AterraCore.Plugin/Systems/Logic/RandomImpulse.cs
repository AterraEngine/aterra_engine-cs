// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Attributes;
using AterraCore.Common.Data;
using AterraCore.Contracts.Nexities.Entities;
using AterraCore.Contracts.OmniVault.World;
using AterraCore.Nexities.Systems;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Workfloor_AterraCore.Plugin.Systems.Logic;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[System(WorkfloorIdLib.SystemsLogic.RandomImpulse, CoreTags.LogicThread)]
[Injectable<RandomImpulse>(ServiceLifetime.Singleton)]
[UsedImplicitly]
public class RandomImpulse : NexitiesSystemWithFilter<IActor2D> {
    protected override Predicate<IActor2D> Filter { get; } = entity => entity is not IPlayer2D;
    private readonly Random _random = new();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override void Tick(ActiveLevel level) {
        foreach (IActor2D entity in GetEntities(level)) {
            entity.Impulse2D.TranslationOffset = entity.Impulse2D.TranslationOffset with {
                X = entity.Impulse2D.TranslationOffset.X + (2 * _random.NextSingle() - 1),
                Y = entity.Impulse2D.TranslationOffset.Y + (2 * _random.NextSingle() - 1)
            };
            float scale = _random.NextSingle();
            entity.Impulse2D.ScaleOffset = entity.Impulse2D.ScaleOffset with {
                X = 1 + 0.01f * MathF.Sign(2 * scale - 1),
                Y = 1 + 0.01f * MathF.Sign(2 * scale - 1)
            };
            entity.Impulse2D.RotationOffset += 2 * _random.NextSingle() - 1;
        }
    }
}
