// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Contracts.Core.Assets;
using AterraEngine.Core.Assets;
using AterraEngine.Lib.ECS.Dtos.Entities;
using AterraEngine.Lib.ECS.Entities;
using Serilog;

namespace Workfloor.Data.Assets;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Asset<Actor2DDto>(16, AssetType.Actor)]
public class DuckyAsset(IAssetAtlas assetAtlas, ILogger logger) : Actor2D(assetAtlas, logger) {
    public override void PopulateFromDto(Actor2DDto assetDto) {
        base.PopulateFromDto(assetDto);
    }
}