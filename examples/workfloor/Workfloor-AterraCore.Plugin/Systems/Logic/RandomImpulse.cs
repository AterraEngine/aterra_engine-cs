// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Attributes;
using AterraCore.Common.Data;
using AterraCore.Contracts.Nexities.Entities;
using AterraCore.Contracts.OmniVault.World;
using AterraCore.Nexities.Systems;
using CodeOfChaos.Extensions.Serilog;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Workfloor_AterraCore.Plugin.Systems.Logic;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[System(WorkfloorIdLib.SystemsLogic.RandomImpulse, CoreTags.LogicThread)]
[Injectable<RandomImpulse>(ServiceLifetime.Singleton)]
[UsedImplicitly]
public class RandomImpulse(ILogger logger) : NexitiesSystemWithFilter<IActor2D> {
    protected override Predicate<IActor2D> Filter { get; } = entity => entity is not IPlayer2D;
    private readonly Random _random = new();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override void Tick(ActiveLevel level) {
        foreach (IActor2D entity in GetEntities(level)) {
            entity.Impulse2D.TranslationOffset = entity.Impulse2D.TranslationOffset with {
                X = entity.Impulse2D.TranslationOffset.X + (2 * _random.NextSingle() - 1) * 10f,
                Y = entity.Impulse2D.TranslationOffset.Y + (2 * _random.NextSingle() - 1) * 10f
            };
            entity.Impulse2D.ScaleOffset = entity.Impulse2D.ScaleOffset with {
                X = entity.Impulse2D.ScaleOffset.X * (2 * _random.NextSingle() - 1) * 10f,
                Y = entity.Impulse2D.ScaleOffset.Y * (2 * _random.NextSingle() - 1) * 10f
            };
            entity.Impulse2D.RotationOffset += 2 * _random.NextSingle() - 1;
        }
    }
}
