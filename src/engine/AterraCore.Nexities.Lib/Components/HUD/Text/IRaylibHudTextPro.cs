// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Raylib_cs;
using System.Numerics;

namespace AterraCore.Nexities.Lib.Components.HUD.Text;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IRaylibHudTextPro {
    public Font Font { get; set; }
    public float FontSize { get; set; }
    public Vector2 Origin { get; set; }
    public Vector2 Position { get; set; }
    public float Rotation { get; set; }
    public float Spacing { get; set; }
    public string Text { get; set; }
    public Color Tint { get; set; }
}
