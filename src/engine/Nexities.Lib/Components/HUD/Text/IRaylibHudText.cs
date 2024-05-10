// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Common;
using AterraCore.Contracts.Nexities.Components;
using Raylib_cs;

namespace Nexities.Lib.Components.HUD.Text;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public interface IRaylibHudText : IComponent {
    public string Text { get; set; }
    public Vector2Int Pos { get; set; }
    public int FontSize { get; set; }
    public Color Color { get; set; }
}