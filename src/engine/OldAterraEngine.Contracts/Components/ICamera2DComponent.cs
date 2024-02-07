// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;
using Raylib_cs;

namespace OldAterraEngine.Contracts.Components;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ICamera2DComponent : IComponent {
    public Camera2D Camera { get; set; }
    public void UpdateCameraSpace();
    void UpdateCamera(Vector2 playerPos, float deltaTime);
}