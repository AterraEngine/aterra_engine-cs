// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Raylib_cs;
using System.Numerics;
using AterraEngine.Contracts.Components;

namespace AterraEngine.Lib.Components;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class Transform2DComponent : ITransform2DComponent {
    private float _rotation;
    public float Rot {
        get => _rotation;
        set => _rotation = value % 360;
    }

    private Vector2 _pos = Vector2.Zero;
    public Vector2 Pos {
        get => _pos;
        set {
            if (_pos == value) return;
            _pos = value;
            UpdateBoundingBox();
        }
    }

    private Vector2 _size = Vector2.One;
    public Vector2 Size {
        get => _size;
        set {
            if (_size == value) return;
            _size = value;
            UpdateBoundingBox();
        }
    }

    private Rectangle _cachedBoundingBox;
    public Rectangle BoundingBox => _cachedBoundingBox;
    
    public Vector2 OriginRelative { get; private set; }

    private void UpdateBoundingBox() {
        _cachedBoundingBox.X = Pos.X - Size.X / 2;
        _cachedBoundingBox.Y = Pos.Y - Size.Y / 2;
        _cachedBoundingBox.Size = Size;
        
        OriginRelative = new Vector2(
            _cachedBoundingBox.Width / 2f,
            _cachedBoundingBox.Height / 2f
        );
    }

    public Transform2DComponent() {
        UpdateBoundingBox();
    }
    
}