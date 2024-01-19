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

    public void Draw(Vector2 pos, float rot, Vector2 origin, Vector2 size, Vector2 worldToScreenSpace){
        // Raylib.DrawTexturePro(
        //     Texture, 
        //     SelectionBox, 
        //     new Rectangle(pos.X, pos.Y, size.X, size.Y), // WTF why does this take pos & box, and not only box?
        //     origin, // needed for rotation
        //     rot, 
        //     Tint
        // );
        //
        Vector2 screenPos = pos * worldToScreenSpace;
        Vector2 screenSize = size * worldToScreenSpace;
        Raylib.DrawTexturePro(
            Texture, 
            SelectionBox, 
            new Rectangle(screenPos.X, screenPos.Y, screenSize.X, screenSize.Y),
            origin * worldToScreenSpace, // origin scaled as well
            rot, 
            Tint
        );
    }
    
    public void DrawDebug(Vector2 pos, float rot, Vector2 origin, Vector2 size, Rectangle box, Vector2 worldToScreenSpace){
        // BOUNDING BOX
        Rectangle screenBox = new Rectangle(box.X * worldToScreenSpace.X, box.Y * worldToScreenSpace.Y, size.X * worldToScreenSpace.X, size.Y * worldToScreenSpace.Y);
        Raylib.DrawRectangleLines((int)screenBox.X, (int)screenBox.Y, (int)screenBox.Width, (int)screenBox.Height, Color.RED);

        // ROTATION
        const float length = 2000;
        Vector2 screenPos = pos * worldToScreenSpace;
        Vector2 endPoint = screenPos + new Vector2((float)Math.Cos(Raylib.DEG2RAD * rot), (float)Math.Sin(Raylib.DEG2RAD * rot)) * length; 
        Raylib.DrawLineV(screenPos, endPoint, Color.DARKGREEN);
    }
}