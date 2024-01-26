// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;
using AterraEngine.Contracts.Assets;
using AterraEngine.Contracts.Atlases;
using AterraEngine.DTO.Assets;
using AterraEngine.Types;
using Raylib_cs;

namespace AterraEngine.Assets;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class Level2D(IAssetAtlas assetAtlas, ITexture2DAtlas texture2DAtlas) : ILevel {
    public EngineAssetId Id { get; private set;  }
    public string? InternalName { get; private set; }
    public List<IAssetNode> Assets { get; private set; } = [];
    public Color BufferBackground { get; private set; } = Color.Pink;
    
    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    public void PopulateFromDto(LevelDto levelDto) {
        Id = levelDto.Id;
        InternalName = levelDto.InternalName;
        // Assets = new AssetNode();
        BufferBackground = levelDto.BufferBackground ?? Color.Pink;
    }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void Unload() {
        throw new NotImplementedException();
    }

    public void Load(EngineAssetId player2DId, out IPlayer2D player2D) {
        assetAtlas.TryGetAsset(player2DId, out player2D!);
        
        texture2DAtlas.TryLoadTexture(player2D.Sprite.TextureId);
        texture2DAtlas.TryGetTexture(player2D.Sprite.TextureId, out var spriteTexture);
        
        player2D.Sprite.Texture = spriteTexture!;
        
        GetActors().ToList().ForEach(actor => {
            texture2DAtlas.TryLoadTexture(actor.Sprite.TextureId);
            texture2DAtlas.TryGetTexture(actor.Sprite.TextureId, out var texture2D);
            actor.Sprite.Texture = texture2D;
        });
    }

    public void Draw(Vector2 worldToScreenSpace) {
        foreach (var asset in GetDrawable()) {
            asset.Draw(worldToScreenSpace);
        }
    }

    public void DrawDebug(Vector2 worldToScreenSpace) {
        foreach (var asset in GetDrawable()) {
            asset.DrawDebug(worldToScreenSpace);
        }
    }

    // Call these methods once, and then only on assets is changed
    private IEnumerable<IDrawableComponent> GetDrawable() {
        return Assets.SelectMany(a => a.CachedFlat).OfType<IDrawableComponent>();
    }

    private IEnumerable<IActor> GetActors() {
        return Assets.SelectMany(a => a.Flat()).OfType<IActor>();
    }
    
}