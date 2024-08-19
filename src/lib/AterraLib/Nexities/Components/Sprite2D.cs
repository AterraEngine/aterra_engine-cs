// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraLib.Nexities.Components;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Component<ISprite2D>(AssetIdLib.AterraCore.Components.Sprite2D)]
[UsedImplicitly]
public class Sprite2D : NexitiesComponent, ISprite2D {
    public virtual AssetId TextureAssetId { get; set; } = new();

    private Rectangle _uvSelection  = new(0, 0, 1, 1);
    public virtual Rectangle UvSelection {
        get => _uvSelection;
        set {
            _uvSelection = value;
            UvAndSourceCalculated = null;
        }
    }
    
    public Rectangle? UvAndSourceCalculated { get; set; } 
}
