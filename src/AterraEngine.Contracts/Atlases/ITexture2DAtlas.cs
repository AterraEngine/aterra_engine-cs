// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Collections.ObjectModel;
using AterraEngine.Types;
using Raylib_cs;

namespace AterraEngine.Contracts.Atlases;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ITexture2DAtlas {
    public ReadOnlyDictionary<string, TextureAtlasObject> TextureAtlasObjects { get; }

    public bool TryRegisterTexture(TextureId id);   // allows the texture to be used later
    public bool TryLoadTexture(TextureId id);       // Loads the texture into VRAM
    public bool TryGetTexture(TextureId id, out Texture2D? texture2D);
    public bool TryUnLoadTexture(TextureId id);     // Unloads the texture from VRAM
    
}