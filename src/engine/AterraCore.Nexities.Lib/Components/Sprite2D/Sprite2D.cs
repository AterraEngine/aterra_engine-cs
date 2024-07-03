﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.FlexiPlug.Attributes;
using AterraCore.Nexities.Components;
using Raylib_cs;

namespace AterraCore.Nexities.Lib.Components.Sprite2D;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Component<ISprite2D>("Nexities:Components/Sprite2D")]
[Injectable<ISprite2D>]
public class Sprite2D : NexitiesComponent, ISprite2D {
    public Texture2D? Texture2D { get; set; }
}
