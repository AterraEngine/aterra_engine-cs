// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;
using AterraEngine.Contracts.Components;
using AterraEngine.Core.Types;
using Raylib_cs;

namespace AterraEngine.Lib.Components;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class Draw2DComponent : IDraw2DComponent {
    public TextureId TextureId { get; set; } = null!;
    public Texture2D? Texture { get; set; } = null;
    public Rectangle SelectionBox { get; set; }
    public Color Tint { get; set; } = Color.White;
}