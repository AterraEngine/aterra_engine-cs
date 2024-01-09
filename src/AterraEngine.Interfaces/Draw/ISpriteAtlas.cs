// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;
using Raylib_cs;

namespace AterraEngine.Interfaces.Draw;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ISpriteAtlas {
    public ISpriteAtlas TryAddSprite(string spriteName, Texture2D texture2D, Vector2 size);
    public bool TryGetSprite(string spriteName, out ISprite? sprite);
}