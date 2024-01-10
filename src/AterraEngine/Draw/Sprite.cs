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
    private Rectangle RectangleSource => new(0, 0, Texture.Width, Texture.Height);


    public void Draw(float rot, Rectangle box){
        Raylib.DrawTexturePro(
            Texture, 
            RectangleSource, 
            new Rectangle(box.X + box.Width/2, box.Y + box.Height/2, box.Width,box.Height), 
            new Vector2(box.Width / 2.0f, box.Height / 2.0f), 
            rot, 
            Color.WHITE
        );
    }
    
    public void DrawDebug(float rot, Rectangle box){
        Raylib.DrawRectangleLines((int)box.X, (int)box.Y, (int)box.Width, (int)box.Height, Color.RED);
        
        const float length = 200;
        Vector2 center = new Vector2(box.X + box.Width / 2, box.Y + box.Height / 2);
        Vector2 endPoint = center + new Vector2((float)Math.Cos(Raylib.DEG2RAD * rot), (float)Math.Sin(Raylib.DEG2RAD * rot))* length; 
        
        Raylib.DrawLineV(center, endPoint,  Color.DARKGREEN);
    }
}