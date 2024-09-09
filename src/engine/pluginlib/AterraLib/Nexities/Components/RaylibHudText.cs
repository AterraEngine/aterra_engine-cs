﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraLib.Nexities.Components;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Component<IRaylibHudText>(AssetIdStringLib.AterraLib.Components.RaylibHudText)]
[UsedImplicitly]
public class RaylibHudText : NexitiesComponent, IHudComponent, IRaylibHudText {
    public HudType Type => HudType.Text;
    public string Text { get; set; } = string.Empty;
    public Vector<int> Pos { get; set; }
    public int FontSize { get; set; }
    public Color Color { get; set; }
}