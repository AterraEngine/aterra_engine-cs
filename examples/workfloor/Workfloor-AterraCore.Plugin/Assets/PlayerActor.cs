// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Contracts.Nexities.Data.Components.AssetTree;
using AterraCore.FlexiPlug.Attributes;
using AterraCore.Nexities.Attributes;
using JetBrains.Annotations;
using AterraCore.Nexities.Lib.Components.Sprite2D;
using AterraCore.Nexities.Lib.Components.Transform2D;
using AterraCore.Nexities.Lib.Entities.Actor;
using AterraCore.Nexities.Components;
using AterraCore.Nexities.Entities;

namespace Workfloor_AterraCore.Plugin.Assets;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Entity("WorkfloorAterraCore:Entities/Ducky")]
[UsedImplicitly]
public class Actor2DDucky(
    ITransform2D transform2D,
    [ComponentUses("827c3bc1-f688-4301-b342-b8958c1fe892")] ISprite2D sprite2D, 
    IAssetTree childEntities
) : Actor2D(transform2D, sprite2D, childEntities);

// [Entity("1")]
// [UsedImplicitly]
// public class PlayerActor(ITransform2D transform2D, PlayerSprite sprite) : Actor2D(transform2D, sprite);
//
//
// [Component("B", AssetInstanceType.Singleton)] // Services.AddSingleton<TreeSprite>()
// [UsedImplicitly]
// public class TreeSprite : Sprite2D {
//     public string data = "leaves";
// }
//
//
// [Entity("1")] // Services.AddTransient<TreeActor>()
// [UsedImplicitly]
// public class TreeActor(ITransform2D transform2D, TreeSprite sprite) : Actor2D(transform2D, sprite);