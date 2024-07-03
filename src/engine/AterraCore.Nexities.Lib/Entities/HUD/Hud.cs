// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Contracts.Nexities.Data.Components;
using AterraCore.Contracts.Nexities.Data.Components.AssetTree;
using AterraCore.Nexities.Entities;
using JetBrains.Annotations;

namespace AterraCore.Nexities.Lib.Entities.HUD;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
[Entity<IHud>("Nexities:Entities/Hud")]
public class Hud(IAssetTree childEntities, params IComponent[] nestedComponents) : NexitiesEntity(nestedComponents:nestedComponents, childEntities), IHud {
    public IAssetTree ChildEntities => childEntities;
}
