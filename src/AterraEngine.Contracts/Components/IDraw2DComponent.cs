// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;
using AterraEngine.Contracts.Assets;
using AterraEngine.Types;
using Raylib_cs;

namespace AterraEngine.Contracts.Components;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IDraw2DComponent : IComponent {
    public TextureId TextureId { get; set; }
    public Texture2D? Texture { get;set;}
    public Rectangle SelectionBox { get;set;}
    public Color Tint { get;set;}
    
    public void Draw(Vector2 pos, float rot, Vector2 origin, Vector2 size, Vector2 worldToScreenSpace);
}