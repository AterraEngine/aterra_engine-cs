// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.OmniVault.Textures;
using AterraCore.OmniVault.Assets;
using AterraCore.OmniVault.Textures;

namespace AterraLib.OmniVault.Textures;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Texture("AterraLib:OmniVault/Textures/Texture2DAsset")]
public class Texture2DAsset : AssetInstance, ITexture2DAsset {
    public virtual string ImagePath { get; set; } = string.Empty;
    private Texture2D _texture;
    private bool _textureDefined;
    public Vector2 Size { get; set; } = Vector2.Zero;

    public bool TryGetTexture(out Texture2D texture) {
        texture = _texture;
        return _textureDefined;
    }
    
    public bool TrySetTexture(Texture2D texture) {
        if (_textureDefined) return false;
        
        _texture = texture;
        _textureDefined = true;
        
        return _textureDefined;
    }
}
