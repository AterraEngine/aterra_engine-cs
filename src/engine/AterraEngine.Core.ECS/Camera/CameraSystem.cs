// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Contracts.Assets;
using AterraEngine.Contracts.ECS;
using AterraEngine.Contracts.ECS.Camera;
namespace AterraEngine.Core.ECS.Ui;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class CameraSystem<T> : EntitySystem, ICameraSystem<T> where T : IEntity {
    public abstract void Process(T entity, float deltaTime, IAsset target);
}