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
public abstract class CameraSystem<T> : EntitySystem<T>, ICameraSystem<T> where T : IEntity {
    public abstract void Process(T entity, float deltaTime, IAsset target);
}