// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Numerics;
using AterraCore.Common;
using AterraCore.Contracts.Nexities.Components.AssetTree;
using AterraCore.Nexities.Components;
using JetBrains.Annotations;
using Nexities.Lib.Entities.HUD;
using Raylib_cs;

namespace Nexities.Lib.Components.HUD.Text;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Component<IAssetTree>("AC00-0005")]
[UsedImplicitly]
public class RaylibHudText : NexitiesComponent, IHudComponent, IRaylibHudText {
    public string Text { get; set; }
    public Vector2Int Pos { get; set; }
    public int FontSize { get; set; }
    public Color Color { get; set; }
    public HudType Type => HudType.Text;
}