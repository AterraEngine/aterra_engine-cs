// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Numerics;
using AterraCore.Contracts.Nexities.Components.AssetTree;
using AterraCore.Nexities.Components;
using JetBrains.Annotations;
using Raylib_cs;
namespace Nexities.Lib.Components.HUD.Text;

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