// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Nexities.Data.Components;
using System.Numerics;

namespace AterraCore.Nexities.Lib.Components.Transform3D;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ITransform3D : IComponent {
    public Vector3 Translation { get; set; }
    public Vector3 Scale { get; set; }
    public Vector3 Rotation { get; set; }
}
