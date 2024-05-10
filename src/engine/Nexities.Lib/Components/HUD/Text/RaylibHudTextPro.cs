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
    public string Text;
    public Vector2 Position;
    public Vector2 Origin;
    public float Rotation;
    public float FontSize;
    public float Spacing;
    public Color Tint;
}