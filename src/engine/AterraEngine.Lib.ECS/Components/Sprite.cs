// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;
using AterraEngine.Contracts.Lib.ECS.Components;
using AterraEngine.Core.Assets;
using AterraEngine.Core.ECSFramework;
using AterraEngine.Lib.ECS.Dtos.Components;
using Raylib_cs;
using Serilog;

namespace AterraEngine.Lib.ECS.Components;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Asset<SpriteDto>(DefaultComponents.PSprite, AssetType.ECSComponent)]
public class Sprite(ILogger logger) : Component<SpriteDto>(logger), ISprite {
    public Texture2D? Texture { get; set; }
    private Rectangle _selectionBox;
    public Rectangle SelectionBox {
        get => _selectionBox;
        set { _selectionBox = value;  _originRelative = null; }
    }
    
    public Color Tint { get; set; }
    
    private Vector2? _originRelative;
    public Vector2 OriginRelative {
        get {
            if (_originRelative != null) return (Vector2)_originRelative;
            _originRelative = SelectionBox.Size / 2;
            return (Vector2)_originRelative;
        }
    }
    
    public override void PopulateFromDto(SpriteDto assetDto) {
        _selectionBox = assetDto.SelectionBox;
        Tint = assetDto.Tint;
    }
}