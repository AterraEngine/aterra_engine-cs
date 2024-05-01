// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;
using AterraCore.Nexities.Components;
using JetBrains.Annotations;

namespace Workfloor_AterraCore.Assets;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Component("0")]
[UsedImplicitly]
public class Transform2DComponent : Component {
    public required Vector2 Translation { get; set; } = Vector2.Zero;
    public required Vector2 Scale { get; set; } = Vector2.One;
    public required Vector2 Rotation { get; set; } = Vector2.Zero;
}