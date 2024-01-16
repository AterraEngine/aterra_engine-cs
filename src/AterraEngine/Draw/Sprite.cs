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
public class Sprite : ISprite {
    public Texture2D Texture { get; set; }
    public Rectangle SelectionBox { get; set; }
    
    public Color Tint { get; set; } = Color.WHITE;

    public void Draw(Vector2 pos, float rot, Vector2 origin, Vector2 size){
        Raylib.DrawTexturePro(
            Texture, 
            SelectionBox, 
            new Rectangle(pos.X, pos.Y, size.X, size.Y), // WTF why does this take pos & box, and not only box?
            origin, // needed for rotation
            rot, 
            Tint
        );
    }
    
    public void DrawDebug(Vector2 pos, float rot, Vector2 origin, Vector2 size, Rectangle box){
        // BOUNDING BOX
        Raylib.DrawRectangleLines((int)box.X, (int)box.Y, (int)size.X, (int)size.Y, Color.RED);
        
        // ROTATION
        const float length = 2000;
        Vector2 endPoint = pos + new Vector2((float)Math.Cos(Raylib.DEG2RAD * rot), (float)Math.Sin(Raylib.DEG2RAD * rot))* length; 
        Raylib.DrawLineV(pos, endPoint,  Color.DARKGREEN);
    }
}