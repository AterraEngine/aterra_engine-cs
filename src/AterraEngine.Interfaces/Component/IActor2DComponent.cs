// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;
using AterraEngine.Interfaces.Assets;
using AterraEngine.Interfaces.Draw;
using Raylib_cs;

namespace AterraEngine.Interfaces.Component;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IActor2DComponent: IAsset {
    public Vector2 Pos { get; set; }
    public float Rotation { get; set; }
    public Rectangle Box { get; set; }
    public Vector2 Velocity { get; set; }
    public ISprite Sprite { get; set; }
}