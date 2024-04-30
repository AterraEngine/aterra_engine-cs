// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Contracts.Nexities.Assets;
using AterraCore.Nexities.Assets;
using AterraCore.Nexities.Components;
using JetBrains.Annotations;

namespace Workfloor_AterraCore.Assets;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
[Component("0")]
[AssetTag("customTag")]
public class TestComponent(IAssetDto assetDto) : Component<IAssetDto>(assetDto);