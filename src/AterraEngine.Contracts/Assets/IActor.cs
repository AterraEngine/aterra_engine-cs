// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;
using Raylib_cs;

namespace AterraEngine.Contracts.Assets;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IActor : IAsset, IDrawableComponent{
    public Vector2 Pos { get; set; }
    public float Rotation { get; set; }
    public Rectangle Box { get; set; }
    public Vector2 Velocity { get; set; }
    public ISprite Sprite { get; set; }
}