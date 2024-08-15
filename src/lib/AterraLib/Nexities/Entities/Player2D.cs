// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraLib.Nexities.Entities;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Serializable]
[UsedImplicitly]
[Entity<IActor2D>(AssetIdLib.AterraCore.Entities.Hud)]
public class Player2D(ITransform2D transform2D, ISprite2D sprite2D, IDirectChildren childEntities, IImpulse2D impulse2D) 
    : Actor2D(transform2D, sprite2D, childEntities, impulse2D), IPlayer2D;

