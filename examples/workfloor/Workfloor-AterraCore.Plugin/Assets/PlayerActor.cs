// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Nexities.Data.Components;
using AterraCore.Contracts.Nexities.Data.Levels;
using AterraCore.Nexities.Attributes;
using JetBrains.Annotations;
using AterraCore.Nexities.Entities;
using AterraLib.Nexities.Entities;

namespace Workfloor_AterraCore.Plugin.Assets;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IDuckyHypeActor : IActor2D;

[Entity<IDuckyHypeActor>("NexitiesDebug:Entities/DuckyHypeActor")]
[UsedImplicitly]
public class DuckyHypeActor(
    ITransform2D transform2D, 
    [InjectAs("827c3bc1-f688-4301-b342-b8958c1fe892")] ISprite2D sprite2D,
    IAssetTree childEntities 
) : Actor2D(transform2D, sprite2D, childEntities), IDuckyHypeActor;
