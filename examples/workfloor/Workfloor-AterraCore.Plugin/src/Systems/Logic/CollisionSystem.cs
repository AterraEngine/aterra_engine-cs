// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Attributes.Nexities;
using AterraCore.Contracts.Nexities.Entities;
using AterraCore.Contracts.Nexities.Systems;
using AterraCore.Contracts.OmniVault.World;
using AterraCore.Contracts.Threading.CrossData;
using AterraCore.Nexities.Systems;
using JetBrains.Annotations;
using Raylib_cs;
using Serilog;
using System.Numerics;
using Workfloor_AterraCore.Plugin.Components;
using Workfloor_AterraCore.Plugin.Entities;

namespace Workfloor_AterraCore.Plugin.Systems.Logic;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
[System(WorkfloorIdLib.SystemsLogic.Collision)]
public class CollisionSystem(ICrossThreadDataAtlas crossThreadDataAtlas, ILogger logger) : NexitiesSystemUnCached<IHasBoundingCircle>, ILogicSytem {

    public override void Tick(ActiveLevel level) {
        IEnumerable<IHasBoundingCircle> entities = GetEntities(level).ToArray();
        if (!level.ActiveEntityTree.TryGetFirst(predicate: node => node.Value is DuckyPlayerActor, out DuckyPlayerActor? player)) return;

        // Check for collisions with other entities
        foreach (IHasBoundingCircle other in entities) {
            if (player == other) continue;

            // Calculate the distance between the player and the other entity
            float distance = Vector2.Distance(player.Transform2D.Translation, other.Transform2D.Translation);

            // Check if the distance is less than or equal to the sum of their bounding circle radii
            if (distance <= player.BoundingCircle.Radius + other.BoundingCircle.Radius) {
                // Collision detected, update the sprite color and log the collision
                other.Sprite2D.Shade = Color.Red;
                logger.Information("Collision detected between {Player} and {Other}", player.InstanceId, other.InstanceId);
            }
        }


        level.ResetActiveEntityTree();
        crossThreadDataAtlas.LevelChangeBus.NotifyLevelChange();
    }
}
