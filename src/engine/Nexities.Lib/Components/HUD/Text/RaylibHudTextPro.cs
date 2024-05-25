// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Numerics;
using JetBrains.Annotations;
using Raylib_cs;
namespace Nexities.Lib.Components.HUD.Text;

using AterraCore.Contracts.Nexities.Data.Components.AssetTree;
using AterraCore.Nexities.Data.Components;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Component<IAssetTree>("AC00-0004")]
[UsedImplicitly]
public class RaylibHudTextPro : NexitiesComponent {
    public Font Font;
    public float FontSize;
    public Vector2 Origin;
    public Vector2 Position;
    public float Rotation;
    public float Spacing;
    public string Text;
    public Color Tint;
}