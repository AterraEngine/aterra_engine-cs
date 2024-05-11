﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Numerics;
using AterraCore.Nexities.Assets;
using AterraCore.Nexities.Components;
using JetBrains.Annotations;

namespace Nexities.Lib.Components.Transform3D;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Component<ITransform3D>("AC000001")]
[AssetTag("data")]
[UsedImplicitly]
public class Transform3D : NexitiesComponent, ITransform3D {
    public Vector3 Translation { get; set; } = Vector3.Zero;
    public Vector3 Scale { get; set; } = Vector3.One;
    public Vector3 Rotation { get; set; } = Vector3.Zero;
}