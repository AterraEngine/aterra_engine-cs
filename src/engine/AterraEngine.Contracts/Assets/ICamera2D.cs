// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Contracts.Components;
using AterraEngine.Contracts.ECS;
using Raylib_cs;

namespace AterraEngine.Contracts.Assets;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ICamera2D : IEntity {
    public ICamera2DComponent Camera2DComponent { get; }
    
    public Camera2D GetRayLibCamera();
}