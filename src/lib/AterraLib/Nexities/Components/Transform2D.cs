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

    private Vector2 _translation = Vector2.Zero;
    public Vector2 Translation {
        get => _translation;
        set {
            _translation = value;
            _destinationRectangle = null;
        }
    }

    private Vector2 _scale = Vector2.One;
    public Vector2 Scale {
        get => _scale;
        set {
            _scale = value;
            _destinationRectangle = null;
        }
    }

    public Vector2 Rotation { get; set; } = Vector2.Zero;

    private Rectangle? _destinationRectangle;
    public Rectangle DestinationRectangle => _destinationRectangle ??= new Rectangle(Translation, Scale);
}
