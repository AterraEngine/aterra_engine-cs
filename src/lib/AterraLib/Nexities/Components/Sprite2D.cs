// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;

namespace AterraLib.Nexities.Components;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Component<ISprite2D>("AterraLib:Nexities/Components/Sprite2D")]
[UsedImplicitly]
public class Sprite2D : NexitiesComponent, ISprite2D {
    public virtual AssetId TextureAssetId { get; set; } = new();
    public virtual Rectangle Selection { get; set; } = new(0, 0, 1, 1);
}
