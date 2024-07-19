// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Nexities.Data.Levels;

namespace AterraLib.Nexities.Entities;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Serializable]
[UsedImplicitly]
[Entity<IActor2D>("AterraLib:Nexities/Entities/Actor2D")]
public class Actor2D(ITransform2D transform2D, ISprite2D sprite2D, IAssetTree childEntities) : NexitiesEntity(transform2D, sprite2D, childEntities), IActor2D {
    public ITransform2D Transform2D => GetComponent<ITransform2D>(transform2D.AssetId);
    public ISprite2D Sprite2D => GetComponent<ISprite2D>(sprite2D.AssetId);
    public IAssetTree AssetTree => GetComponent<IAssetTree>(childEntities.AssetId);
}
