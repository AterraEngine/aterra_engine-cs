// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;
using AterraEngine.Interfaces.Draw;
using Raylib_cs;

namespace AterraEngine.Interfaces.Component;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IPlayerComponent {
    public Vector2 Pos { get; set; }
    public float Rotation { get; set; }
    public Rectangle BoundingBox { get; set; }
    public Vector2 Velocity { get; set; }
    public ISprite Sprite { get; set; }
    public Dictionary<KeyboardKey, Action> KeyMapping { get; set; }
    
    public void LoadKeyMapping();
    public void Draw();
    public void DrawDebug();
}