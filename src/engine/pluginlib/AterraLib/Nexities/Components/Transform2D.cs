// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Attributes.Nexities;

namespace AterraLib.Nexities.Components;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Component<ITransform2D>(StringAssetIdLib.AterraLib.Components.Transform2D)]
[UsedImplicitly]
public class Transform2D : NexitiesComponent, ITransform2D {

    private Rectangle? _destinationRectangle;

    private Vector2? _rotationOrigin;
    protected virtual Vector2 TranslationCache { get; set; } = Vector2.Zero;

    protected virtual Vector2 ScaleCache { get; set; } = Vector2.One;

    public Vector2 Translation {
        get => TranslationCache;
        set {
            TranslationCache = value;
            _destinationRectangle = null;
        }
    }

    public Vector2 Scale {
        get => ScaleCache;
        set {
            ScaleCache = value;
            _destinationRectangle = null;
            _rotationOrigin = null;
        }
    }

    public float Rotation { get; set; } = 0;
    public Rectangle DestinationRectangle => _destinationRectangle ??= new Rectangle(Translation, Scale);
    public Vector2 RotationOrigin => _rotationOrigin ??= Scale / 2;
}
