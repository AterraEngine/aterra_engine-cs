// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;
using Raylib_cs;

namespace AterraEngine.Interfaces.Draw;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ISprite {
    public Texture2D Texture { get; set; }
    public Rectangle SelectionBox { get; set; }
    public Color Tint { get; set; }

    /// <summary>
    /// Draws a texture with rotation, using a specified source rectangle, origin, and rotation angle.
    /// Called every frame.
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="rot">The rotation angle, in radians.</param>
    /// <param name="origin">The origin point for rotation and scaling.</param>
    /// <param name="box">The destination rectangle where the texture will be drawn.</param>
    public void Draw(Vector2 pos, float rot, Vector2 origin, Vector2 size,Rectangle box);

    /// <summary>
    /// Draws a debug representation of a box with a rotation.
    /// Called every frame.
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="rot">The rotation angle, in radians.</param>
    /// <param name="origin">The origin point for rotation and scaling.</param>
    /// <param name="box">The destination rectangle where the texture will be drawn.</param>
    public void DrawDebug(Vector2 pos, float rot, Vector2 origin, Vector2 size, Rectangle box);
}