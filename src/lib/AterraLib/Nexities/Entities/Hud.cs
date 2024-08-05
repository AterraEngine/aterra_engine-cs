// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraLib.Nexities.Entities;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
[Entity<IHud>("AterraLib:Nexities/Entities/Hud")]
public class Hud(IAssetTree childEntities) : NexitiesEntity(childEntities), IHud {
    public IAssetTree AssetTree => GetComponent<IAssetTree>(childEntities.AssetId);
}
