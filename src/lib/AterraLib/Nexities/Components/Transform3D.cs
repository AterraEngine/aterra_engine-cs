// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraLib.Nexities.Components;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Component<ITransform3D>("AterraLib:Nexities/Components/Transform3D")]
[AssetTag("data")]
[UsedImplicitly]
public class Transform3D : NexitiesComponent, ITransform3D {
    public Vector3 Translation { get; set; } = Vector3.Zero;
    public Vector3 Scale { get; set; } = Vector3.One;
    public Vector3 Rotation { get; set; } = Vector3.Zero;
}
