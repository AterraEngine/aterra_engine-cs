// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;
using AterraEngine.Interfaces.Draw;
using Raylib_cs;

namespace AterraEngine.Interfaces.Component;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IActorComponent: IDrawableComponent {
    public Vector2 Pos { get; set; }
    public float Rotation { get; set; }
    public Rectangle BoundingBox { get; set; }
    public Vector2 Velocity { get; set; }
    public ISprite Sprite { get; set; }
}