// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Contracts.Assets;
namespace AterraEngine.Contracts.ECS.Camera;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ICameraSystemManager : IEntitySystemManager<ICameraSystem<IEntity>> {
    public void UpdateEntities(IEnumerable<IEntity> entities, float deltatime, IAsset target);
}