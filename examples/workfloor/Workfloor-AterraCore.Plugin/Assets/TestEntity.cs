// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Nexities.Assets;
using AterraCore.Nexities.Assets;
using AterraCore.Nexities.Components;
using AterraCore.Nexities.Entities;
using AterraCore.Types;

namespace Workfloor_AterraCore.Plugin.Assets;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Entity("0")]
[UsesComponent<TestComponent>]
public class TestEntity(IAssetDto assetDto) : Entity<IAssetDto>(assetDto) {
    
}