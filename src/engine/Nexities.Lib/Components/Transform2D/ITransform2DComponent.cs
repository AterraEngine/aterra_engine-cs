// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;
using AterraCore.Contracts.Nexities.Components;

namespace Nexities.Lib.Components.Transform2D;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public interface ITransform2DComponent : IComponent {
    public Vector2 Translation { get; set; }
    public Vector2 Scale { get; set; }
    public Vector2 Rotation { get; set; }
}