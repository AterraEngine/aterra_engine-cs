// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using OldAterraEngine.Contracts.Components;
using OldAterraEngine.Contracts.ECS;
using Raylib_cs;

namespace OldAterraEngine.Contracts.Assets;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ICamera2D : IEntity {
    public ICamera2DComponent Camera2DComponent { get; }
    
    public Camera2D GetRayLibCamera();
}