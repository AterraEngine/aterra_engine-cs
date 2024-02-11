// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Contracts.Core.ECSFramework.Events;
using AterraEngine.Core.ECSFramework;
using AterraEngine.Core.ECSFramework.Events;
namespace AterraEngine.Lib.ECS.Events;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class CollisionEvent(Entity collider, Entity collided) : Event {
    public Entity ColliderEntity { get; } = collider;
    public Entity CollidedEntity { get; } = collided;
}