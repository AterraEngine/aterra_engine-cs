// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Nexities.Components;
using AterraCore.Contracts.Nexities.Levels;
using AterraCore.Nexities.Entities;
using JetBrains.Annotations;

namespace Workfloor_AterraCore.Plugin.Assets;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Entity<INexitiesLevel>("NexitiesDebug:Levels/MainLevel")]
[UsedImplicitly]
public class MainLevel(IAssetTree assetTree) : NexitiesEntity(assetTree), INexitiesLevel {
    public IAssetTree AssetTree { get; } = assetTree;
}
