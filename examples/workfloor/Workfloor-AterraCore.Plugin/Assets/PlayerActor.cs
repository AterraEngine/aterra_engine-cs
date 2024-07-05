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
public interface ITransform2DNew : ITransform2D {
    public string Data { get; }
}

public interface IActor2DNew : IActor2D;

[Component<ITransform2D, ITransform2DNew>("WorkfloorAterraEngine:Transform2DNew")]
[UsedImplicitly]
public class Transform2DNew : Transform2D, ITransform2DNew {
    public string Data => "bla-bla";
}

[Entity<IActor2D, IActor2DNew>("NexitiesDebug:Entities/Actor2D")]
[OverridesAssetId("Nexities:Entities/Actor2D")]
[UsedImplicitly]
public class Actor2dNew(
    ITransform2DNew transform2D, 
    [RefersTo("827c3bc1-f688-4301-b342-b8958c1fe892")] ISprite2D sprite2D,
    IAssetTree childEntities 
) : Actor2D(transform2D, sprite2D, childEntities), IActor2DNew;

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