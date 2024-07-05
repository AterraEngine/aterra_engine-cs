// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.FlexiPlug.Attributes;
using AterraCore.Nexities.Components;
using Raylib_cs;
using System.Diagnostics.CodeAnalysis;

namespace AterraCore.Nexities.Lib.Components.Sprite2D;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Component<ISprite2D>("Nexities:Components/Sprite2D")]
[Injectable<ISprite2D>]
public class Sprite2D : NexitiesComponent, ISprite2D {
    private Texture2D? _texture2D;
    public Texture2D? Texture2D {
        get => _texture2D;
        set {
            _texture2D = value;
            _textureRectangle = null;
        }
    }

    private Rectangle? _textureRectangle;
    public Rectangle TextureRectangle => _textureRectangle ??= new Rectangle(0, 0, Texture2D?.Width ?? 0, Texture2D?.Height ?? 0);
}
