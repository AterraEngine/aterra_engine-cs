// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;
using OldAterraEngine.Contracts.Components;
using Raylib_cs;

namespace OldAterraEngine.Lib.Components;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class DrawDebug2DComponent : IDrawDebug2DComponent{
    public void Draw(Vector2 pos, float rot, Vector2 origin, Vector2 size, Vector2 worldToScreenSpace) {
        Vector2 adjustedPos = pos * worldToScreenSpace;
        Vector2 adjustedSize = size * worldToScreenSpace;
        Vector2 adjustedOrigin = origin * worldToScreenSpace;
        
        // Draw bounding box - corrected for origin
        Rectangle screenBox = new Rectangle(
            adjustedPos - adjustedOrigin,
            adjustedSize
        );
        Raylib.DrawRectangleLines(
            (int)screenBox.X, 
            (int)screenBox.Y, 
            (int)screenBox.Width, 
            (int)screenBox.Height, 
            Color.Red
        );

        // Draw rotation line - corrected for origin
        const float length = 2000;
        Vector2 endPoint = adjustedPos + new Vector2((float)Math.Cos(Raylib.DEG2RAD * rot), (float)Math.Sin(Raylib.DEG2RAD * rot)) * length;
        Raylib.DrawLineV(adjustedPos, endPoint, Color.DarkGreen);
    }
}