// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;
using AterraEngine.Contracts.Core;
using AterraEngine.Contracts.Core.ECSFramework.Events;
namespace AterraEngine.Core;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class WorldSpace2D(IEventManager eventManager) : IWorldSpace2D {
    public Vector2 WorldToScreenSpace { get; set; } = Vector2.One;

    
    
}