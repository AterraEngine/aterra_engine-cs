// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Nexities.Components;
using AterraCore.Contracts.Nexities.Entities;
using AterraCore.Nexities.Entities;
using AterraLib.Nexities.Entities;
using JetBrains.Annotations;

namespace Workfloor_AterraCore.Plugin.Assets;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IDuckyPlayerActor : IPlayer2D;

[Entity("Workfloor:ActorDuckyPlayer")]
[UsedImplicitly]
public class DuckyPlayerActor(
    ITransform2D transform2D, 
    SpriteDuckyPlatinum sprite2D,
    IAssetTree childEntities,
    IImpulse2D impulse2D
) : Player2D(transform2D, sprite2D, childEntities,impulse2D), IDuckyPlayerActor;
