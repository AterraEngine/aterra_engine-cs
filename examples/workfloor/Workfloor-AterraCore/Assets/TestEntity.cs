﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Nexities.Attributes;
using AterraCore.Nexities.Entities;
using JetBrains.Annotations;

namespace Workfloor_AterraCore.Assets;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Entity("1")]
[UsedImplicitly]
public class TestEntity(Transform2DComponent transform) : Entity(transform), IHasTransformComponent {
    public Transform2DComponent Transform { get; } = transform;
}