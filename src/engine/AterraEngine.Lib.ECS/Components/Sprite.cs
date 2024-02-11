// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;
using AterraEngine.Contracts.Lib.ECS.Components;
using AterraEngine.Core.ECSFramework;
using Raylib_cs;
namespace AterraEngine.Lib.ECS.Components;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public class Sprite : Component, ISprite {
    public Texture2D? Texture { get; set; }
    private Rectangle _selectionBox;
    public Rectangle SelectionBox {
        get => _selectionBox;
        set { _selectionBox = value;  _originRelative = null; }
    }
    
    public Color Tint { get; set; } = Color.White;
    
    private Vector2? _originRelative;
    public Vector2 OriginRelative {
        get {
            if (_originRelative != null) return (Vector2)_originRelative;
            _originRelative = SelectionBox.Size / 2;
            return (Vector2)_originRelative;
        }
    }
}