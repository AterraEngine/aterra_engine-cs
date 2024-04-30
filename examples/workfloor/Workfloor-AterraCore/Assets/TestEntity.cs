// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Contracts.Nexities.Assets;
using AterraCore.Nexities.Attributes;
using AterraCore.Nexities.Entities;
using JetBrains.Annotations;

namespace Workfloor_AterraCore.Assets;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Entity("1")]
[UsesComponent<TestComponent>]
[UsedImplicitly]
public class TestEntity(IAssetDto assetDto) : Entity<IAssetDto>(assetDto);