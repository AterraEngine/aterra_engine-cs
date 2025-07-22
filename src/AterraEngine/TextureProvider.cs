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

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public bool TryGetTexture(string id, out Texture2D texture)
        => _textures.TryGetValue(id, out texture);
    
    public bool TryAddTexture(string id, string path) {
        try {
            if (_textures.ContainsKey(id)) return false;
            if (!Path.Exists(path)) return false;
            
            Texture2D texture = Raylib.LoadTexture(path);
            _textures.Add(id, texture);
            return true;
        }
        catch (Exception e) {
            Console.WriteLine(e);
            return false;
        }
    }
}