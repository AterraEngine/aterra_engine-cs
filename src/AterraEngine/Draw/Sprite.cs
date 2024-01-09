// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;
using AterraEngine.Interfaces.Draw;
using Raylib_cs;

namespace AterraEngine.Draw;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class Sprite :ISprite {
    public Texture2D Texture { get; set; }
    public Vector2 Size { get; set; }

    public void Draw(Vector2 pos) {
        Rectangle sourceRec = new Rectangle(0, 0, Texture.Width, Texture.Height);
        Rectangle destRec = new Rectangle(0,0, 100, 100);
        
        Raylib.DrawTexturePro(Texture, sourceRec, destRec, -pos, 0, Color.WHITE);
    }
}