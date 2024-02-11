// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Contracts.Core.ECSFramework;
using AterraEngine.Contracts.Lib.ECS.Components;
namespace AterraEngine.Contracts.Lib.ECS.ComponentComposites;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IDrawable2D : IEntity {
    public ITransform2D Transform2D {get;}
    public ISprite Sprite {get;}
}