// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Common.Nexities;
using JetBrains.Annotations;
using AterraCore.Nexities.Lib.Components.Sprite2D;
namespace Workfloor_AterraCore.Plugin.Assets;

using AterraCore.Nexities.Components;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Component("A", ServiceLifetimeType.Singleton)] // Services.AddSingleton<PlayerSprite>()
[UsedImplicitly]
public class PlayerSprite : Sprite2D {
    public string data = "bla-bla";
}
//
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