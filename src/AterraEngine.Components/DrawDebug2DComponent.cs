// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;
using AterraEngine.Contracts.Assets;
using AterraEngine.Contracts.Components;
using AterraEngine.Types;
using Raylib_cs;

namespace AterraEngine.Components;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class DrawDebug2DComponent : IDrawDebug2DComponent{
    public void Draw(Vector2 pos, float rot, Vector2 origin, Vector2 size, Vector2 worldToScreenSpace) {
        // BOUNDING BOX
        var screenPosX = pos.X * worldToScreenSpace.X;
        var screenPosY = pos.Y * worldToScreenSpace.Y;
        var screenSizeX = size.X * worldToScreenSpace.X;
        var screenSizeY = size.Y * worldToScreenSpace.Y;

        // BOUNDING BOX
        var screenBox = new Rectangle(pos.X * worldToScreenSpace.X, pos.Y * worldToScreenSpace.Y, screenSizeX, screenSizeY);
        Raylib.DrawRectangleLines((int)screenBox.X, (int)screenBox.Y, (int)screenBox.Width, (int)screenBox.Height, Color.Red);

        // ROTATION
        const float length = 2000;
        Vector2 screenPos = new Vector2(screenPosX, screenPosY);
        Vector2 endPoint = screenPos + new Vector2((float)Math.Cos(Raylib.DEG2RAD * rot), (float)Math.Sin(Raylib.DEG2RAD * rot)) * length;
        Raylib.DrawLineV(screenPos, endPoint, Color.DarkGreen);
    }
}