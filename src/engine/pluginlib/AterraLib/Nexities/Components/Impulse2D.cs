// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Attributes;

namespace AterraLib.Nexities.Components;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Component<IImpulse2D>(StringAssetIdLib.AterraLib.Components.Impulse2D)]
[UsedImplicitly]
public class Impulse2D : NexitiesComponent, IImpulse2D {
    public bool IsEmpty => TranslationOffset == Vector2.Zero && ScaleOffset == Vector2.One && RotationOffset == 0;
    public bool IsNotEmpty => TranslationOffset != Vector2.Zero && ScaleOffset != Vector2.One && RotationOffset != 0;

    public Vector2 TranslationOffset { get; set; } = Vector2.Zero;
    public Vector2 ScaleOffset { get; set; } = Vector2.One;
    public float RotationOffset { get; set; }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void Clear() {
        TranslationOffset = Vector2.Zero;
        ScaleOffset = Vector2.One;
        RotationOffset = 0;
    }
}
