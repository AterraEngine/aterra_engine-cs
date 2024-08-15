// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraLib.Nexities.Entities;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
[Entity<IHud>(AssetIdLib.AterraCore.Entities.Hud)]
public class Hud(IDirectChildren childEntities) : NexitiesEntity(childEntities), IHud {
    public IDirectChildren ChildrenIDs => GetComponent<IDirectChildren>(childEntities.AssetId);
}
