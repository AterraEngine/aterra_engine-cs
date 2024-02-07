// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using OldAterraEngine.Contracts.Components;
using OldAterraEngine.Core.Types;
using Raylib_cs;

namespace OldAterraEngine.Lib.Components;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class Draw2DComponent : IDraw2DComponent {
    public TextureId TextureId { get; set; } = null!;
    public Texture2D? Texture { get; set; } = null;
    public Rectangle SelectionBox { get; set; }
    public Color Tint { get; set; } = Color.White;
}