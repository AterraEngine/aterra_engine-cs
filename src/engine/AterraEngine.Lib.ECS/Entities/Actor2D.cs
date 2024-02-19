// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Contracts.Core.Assets;
using AterraEngine.Contracts.Lib.ECS.Components;
using AterraEngine.Contracts.Lib.ECS.Entities;
using AterraEngine.Core.ECSFramework;
using AterraEngine.Core.Types;
using AterraEngine.Lib.ECS.Components;
namespace AterraEngine.Lib.ECS.Entities;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class Actor2D : Entity, IActor2D {

    public ITransform2D Transform2D => GetComponent<Transform2D>();
    public ISprite Sprite => GetComponent<Sprite>();
    
    public Actor2D(AssetId id, AssetType? assetType) : base(id, assetType ?? AssetType.StaticActor) {
        TryAddComponent<Transform2D>();
        TryAddComponent<Sprite>();
    }
}