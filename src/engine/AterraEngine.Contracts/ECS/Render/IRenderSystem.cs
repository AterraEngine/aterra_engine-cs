// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Contracts.Assets;
using AterraEngine.Contracts.Components;
namespace AterraEngine.Contracts.ECS.Render;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IRenderSystem<T> : IEntitySystem<T> where T : IEntity {
    
    void LoadTextures(IAsset asset);
    void UnloadTextures(IAsset asset);

    void Process(IEntity entity, float deltaTime, ICamera2DComponent camera2DComponent);
}