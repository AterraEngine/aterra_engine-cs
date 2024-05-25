// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Numerics;
using JetBrains.Annotations;
using Raylib_cs;
namespace AterraCore.Nexities.Lib.Components.HUD.Text;

using Nexities.Components;

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