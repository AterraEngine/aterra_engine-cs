// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraLib.Nexities.Components;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Component<ITransform2D>("AterraLib:Nexities/Components/Transform2D")]
[UsedImplicitly]
public class Transform2D : NexitiesComponent, ITransform2D {
    public Vector2 Translation { get; set; } = Vector2.Zero;
    public Vector2 Scale { get; set; } = Vector2.One;
    public Vector2 Rotation { get; set; } = Vector2.Zero;
}
