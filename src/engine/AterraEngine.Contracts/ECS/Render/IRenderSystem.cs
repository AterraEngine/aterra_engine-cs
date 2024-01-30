// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Contracts.Assets;
using AterraEngine.Contracts.Components;
namespace AterraEngine.Contracts.ECS.Render;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IRenderSystem<in T> : IEntitySystem where T : IEntity {
    
    void LoadTextures(IAsset asset);
    void UnloadTextures(IAsset asset);

    void Process(T entity, float deltaTime, ICamera2DComponent camera2DComponent);
}