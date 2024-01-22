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
        var scaledOrigin = origin * worldToScreenSpace;
        var screenPos = pos * worldToScreenSpace;
        Raylib.DrawTexturePro(
            Texture,
            SelectionBox,
            new Rectangle(screenPos.X, screenPos.Y, size.X * worldToScreenSpace.X, size.Y * worldToScreenSpace.Y),
            scaledOrigin, // we use the scaledOrigin here.
            rot,
            Tint
        );
    }
    
    public void DrawDebug(Vector2 pos, float rot, Vector2 origin, Vector2 size, Rectangle box, Vector2 worldToScreenSpace){
        // BOUNDING BOX
        var screenPosX = pos.X * worldToScreenSpace.X;
        var screenPosY = pos.Y * worldToScreenSpace.Y;
        var screenSizeX = size.X * worldToScreenSpace.X;
        var screenSizeY = size.Y * worldToScreenSpace.Y;

        // BOUNDING BOX
        var screenBox = new Rectangle(box.X * worldToScreenSpace.X, box.Y * worldToScreenSpace.Y, screenSizeX, screenSizeY);
        Raylib.DrawRectangleLines((int)screenBox.X, (int)screenBox.Y, (int)screenBox.Width, (int)screenBox.Height, Color.RED);

        // ROTATION
        const float length = 2000;
        Vector2 screenPos = new Vector2(screenPosX, screenPosY);
        Vector2 endPoint = screenPos + new Vector2((float)Math.Cos(Raylib.DEG2RAD * rot), (float)Math.Sin(Raylib.DEG2RAD * rot)) * length;
        Raylib.DrawLineV(screenPos, endPoint, Color.DARKGREEN);
    }
}