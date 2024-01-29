// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;
using AterraEngine.OldContracts.Assets;
using AterraEngine.OldContracts.Components;
using AterraEngine.Types;
using Raylib_cs;

namespace AterraEngine.Components;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class Draw2DComponent : IDraw2DComponent{
    public TextureId TextureId { get; set; } = null!;
    public Texture2D? Texture { get; set; } = null;
    public Rectangle SelectionBox { get; set; }
    public Color Tint { get; set; } = Color.White;

    public void Draw(Vector2 pos, float rot, Vector2 origin, Vector2 size, Vector2 worldToScreenSpace) {
        Vector2 adjustedPos = pos * worldToScreenSpace;
        Vector2 adjustedSize = size * worldToScreenSpace;
        Vector2 adjustedOrigin = origin * worldToScreenSpace;
    
        Raylib.DrawTexturePro(
            Texture ?? default,
            SelectionBox,
            new Rectangle(adjustedPos.X, adjustedPos.Y, adjustedSize.X, adjustedSize.Y),
            adjustedOrigin,
            rot,
            Tint
        );
    }
}