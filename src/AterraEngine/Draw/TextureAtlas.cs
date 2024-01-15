// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Interfaces.Draw;
using Raylib_cs;

namespace AterraEngine.Draw;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class TextureAtlas : ITextureAtlas {
    private readonly Dictionary<string, Texture2D> _textures = new();
       
    public bool TryAddTexture(string textureName, string texturePath) {
        try {
            Texture2D texture2D = Raylib.LoadTexture(texturePath);
            if (_textures.TryAdd(textureName, texture2D)) return true;
            
            // texture could not be added, ergo unload it else it will remain in VRAM forever
            Raylib.UnloadTexture(texture2D);
            return false;
            
        } catch (FileNotFoundException) {
            return false;
        }
    }

    public bool TryGetTexture(string textureName, out Texture2D? texture2D) {
        texture2D = null;
        if (!_textures.TryGetValue(textureName, out var tempTexture)) return false;
        texture2D = tempTexture;
        return true;
    }
 
}