﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Nexities.Components;
using AterraCore.Contracts.Nexities.Entities;
using AterraCore.Contracts.Nexities.Levels;

namespace AterraLib.Nexities.Entities;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
[Entity<IHud>("AterraLib:Nexities/Entities/Hud")]
public class Hud(IDirectChildren childEntities) : NexitiesEntity(childEntities), IHud {
    public IDirectChildren ChildrenIDs => GetComponent<IDirectChildren>(childEntities.AssetId);
}
