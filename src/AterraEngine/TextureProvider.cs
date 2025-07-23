// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraEngine.Contracts;
using CodeOfChaos.Extensions.DependencyInjection;
using Raylib_cs;

namespace AterraEngine;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableSingleton<ITextureProvider>]
public class TextureProvider : ITextureProvider {
    private readonly Dictionary<string, Texture2D> _textures = new();
    private readonly Dictionary<uint, string> _textureIds = new();
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public bool TryGetTexture(uint id, out Texture2D texture) {
        texture = default;
        return _textureIds.TryGetValue(id, out string? textureId) && _textures.TryGetValue(textureId, out texture);
    }

    public bool TryAddTexture(string id, string path, out Texture2D texture) {
        texture = default;
        try {
            if (_textures.ContainsKey(id)) return false;
            if (!Path.Exists(path)) return false;
            
            texture = Raylib.LoadTexture(path);
            _textures.Add(id, texture);
            _textureIds.Add(texture.Id, id);
            return true;
        }
        catch (Exception e) {
            Console.WriteLine(e);
            return false;
        }
    }

    public bool TryGetId(string readableName, out uint id) {
        if (!_textures.TryGetValue(readableName, out Texture2D value)) {
            id = 0;
            return false;
        }
        id = value.Id;
        return true;
    }
}