// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Attributes.DI;
using AterraCore.Common.Attributes.Nexities;
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
[System("Workfloor:ApplyRandomImpulseCamera", CoreTags.LogicThread)]
[Injectable<RandomImpulseCamera>(ServiceLifetime.Singleton)]
[UsedImplicitly]
public class RandomImpulseCamera : NexitiesSystem<ICamera2D> {
    // -----------------------------------------------------------------------------------------------------------------
    public override void Tick(ActiveLevel level) {
        foreach (ICamera2D entity in GetEntities(level)) {
            entity.Impulse2D.RotationOffset += -1f;
        }
    }
}
