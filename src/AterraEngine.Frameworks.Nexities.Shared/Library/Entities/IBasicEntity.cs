// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Contracts;
using AterraEngine.Frameworks.Nexities.Library.Components;

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