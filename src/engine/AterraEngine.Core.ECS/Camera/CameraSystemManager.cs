// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Contracts.Assets;
using AterraEngine.Contracts.ECS;
using AterraEngine.Contracts.ECS.Camera;
namespace AterraEngine.Core.ECS.Camera;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class CameraSystemManager : EntitySystemManager<ICameraSystem<IEntity>>, ICameraSystemManager {
    public void UpdateEntities(IEnumerable<IEntity> entities, float deltaTime, IAsset target) {
        ForEachEntity(
            entities,
            (system, entity) => system.Process(entity, deltaTime, target)
        );
    }
}