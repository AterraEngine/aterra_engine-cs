// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;
using AterraEngine.Interfaces.Component;
using AterraEngine.Interfaces.Draw;
using Raylib_cs;

namespace AterraEngine.Component;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class Player2DComponent:IPlayerComponent {
    private float _rotation;
    public float Rotation {
        get => _rotation;
        set { 
            while (value < 0) value += 360;
            while (value >= 360) value -= 360;
            _rotation = value;
        }
    }

    private Vector2 _pos = new(0f, 0f);
    public Vector2 Pos {
        get => _pos;
        set { _pos = value; UpdateBoundingBox(); } 
    }

    public Rectangle BoundingBox { get; set; }

    private Vector2 _size  = new (250,250);
    public Vector2 Size {
        get => _size;
        set { _size = value; UpdateBoundingBox(); }
    } // if size  is updated, update bounding box
    
    public Vector2 Velocity { get; set; } = Vector2.One;
    
    public ISprite Sprite { get; set; } = null!;
    
    
    public Dictionary<KeyboardKey, Action> KeyMapping { get; set; } = new();
    
    // -----------------------------------------------------------------------------------------------------------------
    // Constructor
    // -----------------------------------------------------------------------------------------------------------------
    protected Player2DComponent() {
        UpdateBoundingBox();
    }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    private void UpdateBoundingBox() {
        BoundingBox = new Rectangle(
            (-Size.X / 2f) + Pos.X, 
            (-Size.Y / 2f) + Pos.Y, 
            Size.X, 
            Size.Y
        );
    }
    
    public void LoadKeyMapping() {
        foreach ((KeyboardKey key, Action action) in KeyMapping) {
            if (Raylib.IsKeyDown(key)) action();
        }
    }

    public void Draw() {
        Sprite.Draw(Rotation, BoundingBox);
    }

    public void DrawDebug() {
        Sprite.DrawDebug(Rotation, BoundingBox);
    }
    
}