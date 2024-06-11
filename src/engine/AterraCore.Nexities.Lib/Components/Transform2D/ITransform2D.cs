// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Nexities.Data.Components;
using System.Numerics;

namespace AterraCore.Nexities.Lib.Components.Transform2D;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public interface ITransform2D : IComponent {
    public Vector2 Translation { get; set; }
    public Vector2 Scale { get; set; }
    public Vector2 Rotation { get; set; }
}
