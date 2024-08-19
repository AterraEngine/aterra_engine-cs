// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Raylib_cs;
using System.Numerics;

namespace AterraCore.Contracts.Nexities.Components;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IRaylibHudText : INexitiesComponent {
    public string Text { get; set; }
    public Vector<int> Pos { get; set; }
    public int FontSize { get; set; }
    public Color Color { get; set; }
}
