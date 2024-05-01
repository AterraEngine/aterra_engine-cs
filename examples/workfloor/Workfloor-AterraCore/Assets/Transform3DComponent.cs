// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;
using AterraCore.Nexities.Assets;
using AterraCore.Nexities.Components;
using JetBrains.Annotations;

namespace Workfloor_AterraCore.Assets;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Component("2")]
[AssetTag("data")]
[UsedImplicitly]
public class Transform3DComponent : Component {
    public Vector3 Translation { get; set; } = Vector3.Zero;
    public Vector3 Scale { get; set; } = Vector3.One;
    public Vector3 Rotation { get; set; } = Vector3.Zero;
}