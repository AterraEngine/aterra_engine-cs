// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Nexities.Components;
using JetBrains.Annotations;
using Raylib_cs;
using System.Numerics;

namespace AterraCore.Nexities.Lib.Components.HUD.Text;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Component<IRaylibHudTextPro>("AC00-0004")]
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
