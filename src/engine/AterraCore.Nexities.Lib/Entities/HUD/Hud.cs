// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Contracts.Nexities.Data.Components.AssetTree;
using AterraCore.Nexities.Entities;
using JetBrains.Annotations;

namespace AterraCore.Nexities.Lib.Entities.HUD;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
[Entity<IHud>("AE00-0005")]
public class Hud(IAssetTree childEntities) : NexitiesEntity(childEntities), IHud {
    public IAssetTree ChildEntities => childEntities;
}
