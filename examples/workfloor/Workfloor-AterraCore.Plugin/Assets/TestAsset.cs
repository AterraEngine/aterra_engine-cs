// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Contracts.FlexiPlug;
using AterraCore.Contracts.Nexities;
using AterraCore.FlexiPlug.Assets;
using AterraCore.Types;

namespace Workfloor_AterraCore.Plugin.Assets;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Asset(
    "0", 
    startingComponents: [typeof(TestComponent)]
)]
public class TestAsset(IAssetDto assetDto) : Asset(assetDto) {

}