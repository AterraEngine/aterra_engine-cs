// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;
using AterraEngine.Contracts.Assets;
using AterraEngine.Contracts.Components;
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
        Raylib.DrawTexturePro(
            Texture ?? default,
            SelectionBox,
            new Rectangle((pos * worldToScreenSpace).X,
                (pos * worldToScreenSpace).Y,
                (size * worldToScreenSpace).X,
                (size * worldToScreenSpace).Y),
            origin * worldToScreenSpace,
            rot,
            Tint
        );
    }
}