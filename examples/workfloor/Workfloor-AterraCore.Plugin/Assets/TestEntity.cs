﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Nexities.Assets;
using AterraCore.Nexities.Attributes;
using AterraCore.Nexities.Components;
using AterraCore.Nexities.Entities;
using JetBrains.Annotations;

namespace Workfloor_AterraCore.Plugin.Assets;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Entity("0")]
[UsesComponent<TestComponent>]
[UsedImplicitly]
public class TestEntity(IAssetDto assetDto) : Entity<IAssetDto>(assetDto);