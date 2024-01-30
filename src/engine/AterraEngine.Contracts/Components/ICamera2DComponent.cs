// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;
using Raylib_cs;

namespace AterraEngine.Contracts.Components;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ICamera2DComponent : IComponent {
    public Camera2D Camera { get; set; } 
    
    public Vector2 WorldToScreenSpace {get;}
    public Vector2 ScreenToWorldSpace {get;}
    
    void UpdateCamera(Vector2 playerPos, float deltaTime);
}