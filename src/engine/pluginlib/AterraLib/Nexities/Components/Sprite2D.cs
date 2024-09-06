// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraLib.Nexities.Components;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Component<ISprite2D>(AssetIdStringLib.AterraLib.Components.Sprite2D)]
[UsedImplicitly]
public class Sprite2D : NexitiesComponent, ISprite2D {
    public virtual AssetId TextureAssetId { get; set; } = new();

    private Rectangle _uvSelection = new(0, 0, 1, 1);

    public virtual Rectangle UvSelection {
        get => _uvSelection;
        set {
            _uvSelection = value;
            UvAndSourceCalculated = null;
        }
    }
    
    public Color Shade { get; set; } = Color.White;

    public Rectangle? UvAndSourceCalculated { get; set; }
}
