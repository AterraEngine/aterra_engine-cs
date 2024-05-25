// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;

namespace Nexities.Lib.Components.Transform2D;

using AterraCore.Contracts.Nexities.Data.Components;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public interface ITransform2D : IComponent {
    public Vector2 Translation { get; set; }
    public Vector2 Scale { get; set; }
    public Vector2 Rotation { get; set; }
}