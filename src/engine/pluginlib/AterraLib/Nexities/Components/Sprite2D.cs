// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Attributes.Nexities;

namespace AterraLib.Nexities.Components;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Component<ISprite2D>(StringAssetIdLib.AterraLib.Components.Sprite2D)]
[UsedImplicitly]
public class Sprite2D : NexitiesComponent, ISprite2D {

    private Rectangle _uvSelection = new(0, 0, 1, 1);
    public virtual AssetId TextureAssetId { get; set; } = new();

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
