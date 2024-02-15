// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Contracts.Core.ECSFramework.Events;
using AterraEngine.Core.ECSFramework.Events;
using AterraEngine.Lib.ECS.Events;
namespace AterraEngine.Lib.ECS.Systems;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
// TODO add DI EventManager
public class CollisionSystem(IEventManager eventManager) : EventSystem<CollisionEvent> {
    public override void HandleEvent(CollisionEvent ev) {
        // AssetAtlas.Remove(ev.CollidedEntity);
    }
}