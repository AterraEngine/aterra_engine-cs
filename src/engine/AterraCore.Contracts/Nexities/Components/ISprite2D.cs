// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using Raylib_cs;

namespace AterraCore.Contracts.Nexities.Components;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ISprite2D : INexitiesComponent {
    public AssetId TextureAssetId { get; set; }
    public Rectangle UvSelection { get; set; }

    public Rectangle? UvAndSourceCalculated { get; set; }
}
