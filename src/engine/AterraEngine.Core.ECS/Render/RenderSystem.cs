// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Contracts.Assets;
using AterraEngine.Contracts.Atlases;
using AterraEngine.Contracts.Components;
using AterraEngine.Contracts.ECS;
using AterraEngine.Contracts.ECS.Render;
namespace AterraEngine.Core.ECS.Render;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class RenderSystem<T>(ITexture2DAtlas texture2DAtlas) : EntitySystem<T>, IRenderSystem<T> where T : IEntity {
    public void LoadTextures(IAsset asset) {
        if (!asset.TryGetComponent<IDraw2DComponent>(out var draw2D)) return;
        
        texture2DAtlas.TryLoadTexture(draw2D.TextureId);
        texture2DAtlas.TryGetTexture(draw2D.TextureId, out var texture2D);
        draw2D.Texture = texture2D;
    }

    public void UnloadTextures(IAsset asset) {
        if (!asset.TryGetComponent<IDraw2DComponent>(out var draw2D)) return;
        
        texture2DAtlas.TryUnLoadTexture(draw2D.TextureId);
    }
    
    public abstract void Process(IEntity entity, float deltaTime, ICamera2DComponent camera2DComponent);
}