// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Attributes;
using AterraCore.Contracts.Nexities.Components;
using AterraCore.Contracts.Nexities.Entities;
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
    [InjectAs("01J5RA7EDMS1PRR1BMRN9XM9BU")] SpriteDuckyPlatinum sprite2D,
    IDirectChildren childEntities,
    IImpulse2D impulse2D
) : Player2D(transform2D, sprite2D, childEntities,impulse2D), IDuckyPlayerActor;
