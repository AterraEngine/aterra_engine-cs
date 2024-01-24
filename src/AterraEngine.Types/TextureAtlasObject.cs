// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Raylib_cs;

namespace AterraEngine.Types;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class TextureAtlasObject(string filepath) { // YES THIS IS A CLASS, because it is supposed to be a reference type
    public bool IsLoaded { get; set; }
    public string FilePath { get; private set; } = filepath;
    public Texture2D? Texture { get; set; }
}