// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Nexities.Attributes;
using AterraCore.Nexities.Components;
using JetBrains.Annotations;
using System.Numerics;

namespace AterraCore.Nexities.Lib.Components.Transform3D;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Component<ITransform3D>("Nexities:Components/Transform3D")]
[AssetTag("data")]
[UsedImplicitly]
public class Transform3D : NexitiesComponent, ITransform3D {
    public Vector3 Translation { get; set; } = Vector3.Zero;
    public Vector3 Scale { get; set; } = Vector3.One;
    public Vector3 Rotation { get; set; } = Vector3.Zero;
}
