// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;
using AterraCore.Contracts.Nexities.Components;
namespace Nexities.Lib.Components.Transform3D;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ITransform3D : IComponent {
    public Vector3 Translation { get; set; }
    public Vector3 Scale { get; set; }
    public Vector3 Rotation { get; set; }
}