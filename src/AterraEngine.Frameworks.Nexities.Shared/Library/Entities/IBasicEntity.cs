// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Frameworks.Nexities.Library.Components;
using AterraEngine.Shared;

namespace AterraEngine.Frameworks.Nexities.Library.Entities;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IBasicEntity : INexitiesEntity,
    IDeconstructable<ITransformComponent, ISpriteComponent> 
{
    ITransformComponent Transform { get; }
    ISpriteComponent Sprite { get; }
}