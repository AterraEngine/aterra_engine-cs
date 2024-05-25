// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Common;
using JetBrains.Annotations;
using Raylib_cs;
namespace Nexities.Lib.Components.HUD.Text;

using AterraCore.Contracts.Nexities.Data.Components.AssetTree;
using AterraCore.Nexities.Data.Components;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Component<IAssetTree>("AC00-0005")]
[UsedImplicitly]
public class RaylibHudText : NexitiesComponent, IHudComponent, IRaylibHudText {
    public HudType Type => HudType.Text;
    public string Text { get; set; }
    public Vector2Int Pos { get; set; }
    public int FontSize { get; set; }
    public Color Color { get; set; }
}