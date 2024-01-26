// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Collections.ObjectModel;
using AterraEngine.Contracts.Atlases;
using AterraEngine.Types;
using Raylib_cs;

namespace AterraEngine.Atlases;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class Texture2DAtlas : ITexture2DAtlas {
    private Dictionary<string, TextureAtlasObject> _textureAtlasObjects = new();
    public ReadOnlyDictionary<string, TextureAtlasObject> TextureAtlasObjects => _textureAtlasObjects.AsReadOnly();
    
    public bool TryRegisterTexture(TextureId id) {
        return _textureAtlasObjects.TryAdd(id.name, new TextureAtlasObject(id.filePath));
    }
    
    public bool TryLoadTexture(TextureId id) {
        if (!_textureAtlasObjects.TryGetValue(id.name, out var value)) return false;
        value.IsLoaded = true;
        value.Texture = Raylib.LoadTexture(value.FilePath);
        
        return true;
    }

    public bool TryGetTexture(TextureId id, out Texture2D? texture2D) {
        texture2D = null;
        if (!_textureAtlasObjects.TryGetValue(id.name, out var value) 
            || !value.IsLoaded 
            || value.Texture is null
        ) return false;

        texture2D = value.Texture;
        return true;
    }

    public bool TryUnLoadTexture(TextureId id) {
        var result = _textureAtlasObjects.TryGetValue(id.name, out var value);

        Raylib.UnloadTexture((Texture2D)value.Texture!);
        
        value.IsLoaded = false;
        value.Texture = null;
        
        return result;
    }
}