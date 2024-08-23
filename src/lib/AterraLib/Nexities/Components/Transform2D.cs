// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraLib.Nexities.Components;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Component<ITransform2D>(AssetIdLib.AterraCore.Components.Transform2D)]
[UsedImplicitly]
public class Transform2D : NexitiesComponent, ITransform2D {
    protected virtual Vector2 TranslationCache { get; set; } = Vector2.Zero;
    public Vector2 Translation {
        get => TranslationCache;
        set {
            TranslationCache = value;
            _destinationRectangle = null;
        }
    }

    protected virtual Vector2 ScaleCache { get; set; } = Vector2.One;
    public Vector2 Scale {
        get => ScaleCache;
        set {
            ScaleCache = value;
            _destinationRectangle = null;
            _rotationOrigin = null;
        }
    }

    public float Rotation { get; set; } = 0;

    private Rectangle? _destinationRectangle;
    public Rectangle DestinationRectangle => _destinationRectangle ??= new Rectangle(Translation, Scale);
    
    private Vector2? _rotationOrigin;
    public Vector2 RotationOrigin => _rotationOrigin ??= Scale / 2;
}
