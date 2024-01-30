// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Contracts.Assets;
using AterraEngine.Contracts.Components;
using AterraEngine.Contracts.ECS;
using AterraEngine.Contracts.ECS.Render;
namespace AterraEngine.Core.ECS.Render;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class RenderSystem<T> : EntitySystem, IRenderSystem<T> where T : IEntity {
    public abstract void LoadTextures(IAsset asset);
    public abstract void UnloadTextures(IAsset asset);
    public abstract void Process(T entity, float deltaTime, ICamera2DComponent camera2DComponent);
}