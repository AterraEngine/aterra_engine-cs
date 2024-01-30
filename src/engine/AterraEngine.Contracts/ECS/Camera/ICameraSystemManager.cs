// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Contracts.Assets;
using AterraEngine.Contracts.ECS.Camera;
namespace AterraEngine.Contracts.ECS.Logic;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ICameraSystemManager : IEntitySystemManager<ICameraSystem<IEntity>> {
    public void UpdateEntities(IEnumerable<IEntity> entities, float deltatime, IAsset target);
}