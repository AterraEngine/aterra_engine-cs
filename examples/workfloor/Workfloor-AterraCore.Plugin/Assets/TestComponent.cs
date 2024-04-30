// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Nexities.Assets;
using AterraCore.Nexities.Assets;
using AterraCore.Nexities.Components;
using JetBrains.Annotations;

namespace Workfloor_AterraCore.Plugin.Assets;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Component("0")]
[UsedImplicitly]
[AssetTag("customTag")]
public class TestComponent(IAssetDto assetDto) : Component<IAssetDto>(assetDto);

[Component("0")]
[UsedImplicitly]
[AssetTag("customTag")]
public class TestComponent1(IAssetDto assetDto) : Component<IAssetDto>(assetDto);