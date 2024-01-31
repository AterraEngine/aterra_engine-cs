﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Contracts.Assets;
namespace AterraEngine.Contracts.ECS.Camera;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ICameraSystem<T> : IEntitySystem<T> where T : IEntity {
    public void Process(T asset, float deltaTime, IAsset target);
}