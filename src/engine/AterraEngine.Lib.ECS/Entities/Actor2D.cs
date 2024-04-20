// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraEngine.Contracts.Core.Assets;
using AterraEngine.Contracts.Lib.ECS.Components;
using AterraEngine.Contracts.Lib.ECS.Entities;
using AterraEngine.Core.ECSFramework;
using AterraEngine.Lib.ECS.Components;
using AterraEngine.Lib.ECS.Dtos.Entities;
using Serilog;

namespace AterraEngine.Lib.ECS.Entities;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class Actor2D : Entity<Actor2DDto>, IActor2D {

    public ITransform2D Transform2D => GetComponent<ITransform2D>();
    public ISprite Sprite => GetComponent<ISprite>();

    public Actor2D(IAssetAtlas assetAtlas, ILogger logger) : base(assetAtlas, logger) {
        TryAddComponent<ITransform2D>(DefaultComponents.Transform2D);
        TryAddComponent<ISprite>(DefaultComponents.Sprite);
    }
    
    public override void PopulateFromDto(Actor2DDto dto) {
        Transform2D.TryPopulateFromDto(dto.Transform2DDto);
        Sprite.TryPopulateFromDto(dto.SpriteDto);
    }
}