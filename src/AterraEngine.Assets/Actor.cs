// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;
using AterraEngine.Contracts.Assets;
using AterraEngine.Types;
using Raylib_cs;

namespace AterraEngine.Assets;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class Actor : Asset, IActor {
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

    public Rectangle Box { get; set; }

    private Vector2 _size  = new (1,1);
    public Vector2 Size {
        get => _size;
        set { _size = value; UpdateBoundingBox(); }
    } // if size  is updated, update bounding box

    public Vector2 OriginRelative {
        get;
        private set;
    }

    public Vector2 Velocity { get; set; } = Vector2.One;

    public ISprite Sprite { get; set; } = null!;
    
    // -----------------------------------------------------------------------------------------------------------------
    // Constructor
    // -----------------------------------------------------------------------------------------------------------------
    public Actor(EngineAssetId id, string? internalName) : base(id, internalName) {
        UpdateBoundingBox();
    }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    internal void UpdateBoundingBox() {
        Box = new Rectangle(
            (-Size.X / 2f) + Pos.X, 
            (-Size.Y / 2f) + Pos.Y, 
            Size.X,
            Size.Y
        );

        OriginRelative = new Vector2(
            Box.Width/2f,
            Box.Height/2f
        );
    }

    public override void Draw(Vector2 worldToScreenSpace) {
        Sprite.Draw(Pos, Rotation, OriginRelative, Size, worldToScreenSpace);
    }

    public override void DrawDebug(Vector2 worldToScreenSpace) {
        Sprite.DrawDebug(Pos, Rotation, OriginRelative, Size, Box, worldToScreenSpace);
    }

}