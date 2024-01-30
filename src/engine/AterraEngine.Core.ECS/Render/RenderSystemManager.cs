// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Contracts.Components;
using AterraEngine.Contracts.ECS;
using AterraEngine.Contracts.ECS.Render;

namespace AterraEngine.Core.ECS.Render;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class RenderSystemManager : EntitySystemManager<IRenderSystem<IEntity>>, IRenderSystemManager {
    public void UpdateEntities(IEnumerable<IEntity> entities, float deltaTime, ICamera2DComponent camera2DComponent) {
        ForEachEntity(
            entities,
            (system, entity) => system.Process(entity, deltaTime, camera2DComponent)
        );
    }
}