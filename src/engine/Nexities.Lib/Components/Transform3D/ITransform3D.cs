// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;

namespace Nexities.Lib.Components.Transform3D;

using AterraCore.Contracts.Nexities.Data.Components;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ITransform3D : IComponent {
    public Vector3 Translation { get; set; }
    public Vector3 Scale { get; set; }
    public Vector3 Rotation { get; set; }
}