// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Contracts.Assets;
using AterraEngine.Contracts.Atlases;
using AterraEngine.Contracts.Components;
using AterraEngine.Contracts.Systems;

namespace AterraEngine.Lib.ComponentSystems;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class Render2DSystem(ITexture2DAtlas texture2DAtlas) : IRenderSystem {

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

    public void Process(IAsset asset, float deltaTime, ICamera2DComponent camera2DComponent) {
        if (!asset.TryGetComponent<ITransform2DComponent>(out var transform2D)) return;
        if (!asset.TryGetComponent<IDraw2DComponent>(out var draw2D)) return;
        
        // Apply the rotation to the movement
        draw2D.Draw(
            transform2D.Pos,
            transform2D.Rot,
            transform2D.OriginRelative,
            transform2D.Size,
            camera2DComponent.WorldToScreenSpace
        );
        
        // if (!asset.TryGetComponent<IDrawDebug2DComponent>(out var drawDebug2D)) return;
        // drawDebug2D.Draw(
        //     transform2D.Pos,
        //     transform2D.Rot,
        //     transform2D.OriginRelative,
        //     transform2D.Size,
        //     camera2DComponent.WorldToScreenSpace
        // );
    }
}