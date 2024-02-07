// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using OldAterraEngine.Core.Types;
using Raylib_cs;

namespace OldAterraEngine.Contracts.Components;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IDraw2DComponent : IComponent {
    public TextureId TextureId { get; set; }
    public Texture2D? Texture { get;set;}
    public Rectangle SelectionBox { get;set;}
    public Color Tint { get;set;}
}