// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;
using AterraEngine.Contracts.Actors;
using AterraEngine.Contracts.Atlases;
using AterraEngine.Types;
using Raylib_cs;

namespace AterraEngine.Actors;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class Level2D(IAssetAtlas assetAtlas, ITexture2DAtlas texture2DAtlas) : ILevel {
    public EngineAssetId Id { get; init;  }
    public string? InternalName { get; init; }
    public List<IActor> Actors { get; init; } = [];
    public Color BufferBackground { get; init; } = Color.Pink;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void Unload() {
        throw new NotImplementedException();
    }

    public void Load(EngineAssetId player2DId, IAssetAtlas assetAtlas, out IPlayer2D player2D) {
        assetAtlas.TryGetAsset(player2DId, out player2D!);
        
        texture2DAtlas.TryLoadTexture(player2D.Sprite.TextureId);
        texture2DAtlas.TryGetTexture(player2D.Sprite.TextureId, out var spriteTexture);
        
        player2D.Sprite.Texture = spriteTexture!;
        
        Actors.ToList().ForEach(actor => {
            texture2DAtlas.TryLoadTexture(actor.Sprite.TextureId);
            texture2DAtlas.TryGetTexture(actor.Sprite.TextureId, out var texture2D);
            actor.Sprite.Texture = texture2D;
        });


    }
    
    public void Draw(Vector2 worldToScreenSpace) {
        throw new NotImplementedException();
    }

    public void DrawDebug(Vector2 worldToScreenSpace) {
        throw new NotImplementedException();
    }
    
}