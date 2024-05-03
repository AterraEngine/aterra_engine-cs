﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Numerics;

namespace Nexities.Lib.Components.Transform3D;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public interface ITransform3DComponent {
    public Vector3 Translation { get; set; }
    public Vector3 Scale { get; set; }
    public Vector3 Rotation { get; set; }
}