// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using JetBrains.Annotations;
namespace Nexities.Lib.Entities.HUD;

using AterraCore.Contracts.Nexities.Data.Components.AssetTree;
using AterraCore.Nexities.Data.Entities;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
[Entity<IHud>("AE00-0005")]
public class Hud(IAssetTree childEntities) : NexitiesEntity(childEntities), IHud {
    public IAssetTree ChildEntities => childEntities;
}