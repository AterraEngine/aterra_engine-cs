// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Contracts.Components;
namespace AterraEngine.Contracts.ECS.Render;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IRenderSystemManager {
    public bool TryAdd(IRenderSystem<IEntity> system);
    public void UpdateEntities(IEnumerable<IEntity> entities, float deltatime, ICamera2DComponent camera2DComponent);
}