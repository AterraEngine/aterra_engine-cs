// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraLib.Nexities.Components;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Component<IImpulse2D>(AssetIdLib.AterraLib.Components.Impulse2D)]
[UsedImplicitly]
public class Impulse2D : NexitiesComponent, IImpulse2D {
    private Vector2 _translationOffset = Vector2.Zero;
    private Vector2 _scaleOffset = Vector2.Zero;
    private float _rotationOffset;
    public bool IsEmpty { get; private set; } = true;
    public bool IsNotEmpty => !IsEmpty;

    public Vector2 TranslationOffset {
        get => _translationOffset;
        set {
            _translationOffset = value;
            if (value != Vector2.Zero) IsEmpty = false;
        }
    }

    public Vector2 ScaleOffset {
        get => _scaleOffset;
        set {
            _scaleOffset = value;
            if (value != Vector2.Zero) IsEmpty = false;
        }
    }

    public float RotationOffset {
        get => _rotationOffset;
        set {
            _rotationOffset = value;
            if (value != 0) IsEmpty = false;
        }
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void Clear() {
        TranslationOffset = Vector2.Zero;
        ScaleOffset = Vector2.Zero;
        RotationOffset = 0;
    }
}
