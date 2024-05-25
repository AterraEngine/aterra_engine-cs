// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Common;
using Raylib_cs;
namespace AterraCore.Nexities.Lib.Components.HUD.Text;

using AterraCore.Contracts.Nexities.Data.Components;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public interface IRaylibHudText : IComponent {
    public string Text { get; set; }
    public Vector2Int Pos { get; set; }
    public int FontSize { get; set; }
    public Color Color { get; set; }
}