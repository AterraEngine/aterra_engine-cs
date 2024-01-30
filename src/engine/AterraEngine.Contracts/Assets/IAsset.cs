﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraEngine.Contracts.Components;
using AterraEngine.Contracts.DTOs.Components;
using AterraEngine.Contracts.ECS;

namespace AterraEngine.Contracts.Assets;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IAsset : IEntity {
    IInputComponent<Input2DDto> InputComponent { get; }
    IMovement2DComponent Movement2DComponent { get; }
}