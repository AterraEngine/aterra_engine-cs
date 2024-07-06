// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Nexities.Data.Components;
using AterraCore.Contracts.OmniVault;
using Raylib_cs;

namespace AterraCore.Nexities.Lib.Components.Sprite2D;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ISprite2D : IComponent, ITexture2DAsset {
    public Rectangle TextureRectangle { get; }
}
