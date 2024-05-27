// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using Raylib_cs;
namespace AterraCore.Nexities.Lib.Components.HUD.Text;

using AterraCore.Contracts.Nexities.Data.Components;
using System.Numerics;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public interface IRaylibHudText : IComponent {
    public string Text { get; set; }
    public Vector<int> Pos { get; set; }
    public int FontSize { get; set; }
    public Color Color { get; set; }
}