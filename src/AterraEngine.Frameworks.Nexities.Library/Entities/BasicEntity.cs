// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraEngine.Frameworks.Nexities.Library.Components;

namespace AterraEngine.Frameworks.Nexities.Library.Entities;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class BasicEntity : NexitiesEntity<BasicEntity>, IBasicEntity {
    public ITransformComponent Transform => GetComponent<ITransformComponent>(0);
    public ISpriteComponent Sprite =>  GetComponent<ISpriteComponent>(1);

    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    public BasicEntity() : base(2) {
        SetComponent<TransformComponent>(0);
        SetComponent<SpriteComponent>(1);
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void Deconstruct(out ITransformComponent transform, out ISpriteComponent sprite) {
        transform = Transform;
        sprite = Sprite;
    }
}