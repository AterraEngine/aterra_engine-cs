// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using JetBrains.Annotations;
using Raylib_cs;
namespace AterraCore.Nexities.Lib.Components.HUD.Text;

using Nexities.Components;
using System.Numerics;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Component<IRaylibHudText>("AC00-0005")]
[UsedImplicitly]
public class RaylibHudText : NexitiesComponent, IHudComponent, IRaylibHudText {
    public HudType Type => HudType.Text;
    public string Text { get; set; } = string.Empty;
    public Vector<int> Pos { get; set; }
    public int FontSize { get; set; }
    public Color Color { get; set; }
}