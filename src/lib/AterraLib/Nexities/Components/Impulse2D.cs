// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraLib.Nexities.Components;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Component<IImpulse2D>("AterraLib:Nexities/Components/Impulse2D")]
[UsedImplicitly]
public class Impulse2D : NexitiesComponent, IImpulse2D {
    public Vector2 TranslationOffset { get; set; } = Vector2.Zero;
    public Vector2 ScaleOffset { get; set; } = Vector2.Zero;
    public float RotationOffset { get; set; }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void Clear() {
        TranslationOffset = Vector2.Zero;
        ScaleOffset = Vector2.Zero;
        RotationOffset = 0;
    }
}
