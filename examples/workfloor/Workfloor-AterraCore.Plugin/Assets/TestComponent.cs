// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Nexities.Assets;
using AterraCore.Nexities.Components;
using JetBrains.Annotations;

namespace Workfloor_AterraCore.Plugin.Assets;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Component("0")]
[UsedImplicitly]
public class TestComponent(IAssetDto assetDto) : Component<IAssetDto>(assetDto);

[Component("2")] [UsedImplicitly] public class TestComponent2(IAssetDto assetDto) : Component<IAssetDto>(assetDto);
[Component("3")] [UsedImplicitly] public class TestComponent3(IAssetDto assetDto) : Component<IAssetDto>(assetDto);