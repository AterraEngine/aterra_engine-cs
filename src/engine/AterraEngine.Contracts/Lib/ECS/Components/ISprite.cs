// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;
using AterraEngine.Contracts.Core.ECSFramework;
using Raylib_cs;
namespace AterraEngine.Contracts.Lib.ECS.Components;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ISprite : IComponent  {
    Texture2D? Texture { get; set; }
    Rectangle SelectionBox { get; set; }
    Color Tint { get; set; }
    Vector2 OriginRelative { get; }
}