// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;
using Raylib_cs;
namespace AterraEngine.Contracts.DTOs.ECS;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public readonly struct EcsUpdateDtoRender(float deltaTime, Vector2 worldToScreenSpace) : IEcsUpdateDto {
    public float DeltaTime { get;}= deltaTime;
    public Vector2 WorldToScreenSpace { get; } = worldToScreenSpace;
}