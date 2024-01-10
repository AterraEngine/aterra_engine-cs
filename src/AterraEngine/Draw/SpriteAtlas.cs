// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Drawing;
using System.Numerics;
using AterraEngine.Interfaces.Draw;
using AterraEngine.Services;
using Raylib_cs;

namespace AterraEngine.Draw;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class SpriteAtlas : ISpriteAtlas {
    private Dictionary<string, ISprite> _dictionary = new();

    public ISpriteAtlas TryAddSprite(string spriteName,Texture2D texture2D, Vector2 size) {
        ISprite? sprite = EngineServices.GetService<ISprite>();
        sprite.Texture = texture2D;
        // sprite.Size = size;
        _dictionary.Add(spriteName, sprite);

        return this;
    }

    public bool TryGetSprite(string spriteName, out ISprite? sprite) {
        return _dictionary.TryGetValue(spriteName, out sprite);
    }
}