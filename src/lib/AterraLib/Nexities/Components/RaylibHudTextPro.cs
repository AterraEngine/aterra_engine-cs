﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraLib.Nexities.Components;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Component<IRaylibHudTextPro>("AterraLib:Nexities/Components/RaylibHubTextPro")]
[UsedImplicitly]
public class RaylibHudTextPro : NexitiesComponent, IRaylibHudTextPro {
    public Font Font { get; set; }
    public float FontSize { get; set; }
    public Vector2 Origin { get; set; }
    public Vector2 Position { get; set; }
    public float Rotation { get; set; }
    public float Spacing { get; set; }
    public string Text { get; set; } = string.Empty;
    public Color Tint { get; set; }
}