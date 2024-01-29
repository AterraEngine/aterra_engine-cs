// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;
using Raylib_cs;

namespace AterraEngine.OldContracts.Components;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public interface ITransform2DComponent : IComponent{
    public Vector2 Pos { get; set; }
    public Vector2 Size { get; set; }
    public float Rot { get; set; }
    public Rectangle BoundingBox { get; }
    public Vector2 OriginRelative { get; }
}