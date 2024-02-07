﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Contracts.Components;
namespace AterraEngine.Contracts.ECS.EntityTypes;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IMovementEntity : IEntity{
    public IMovement2DComponent Movement { get; }
}