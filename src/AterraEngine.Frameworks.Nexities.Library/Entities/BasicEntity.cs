// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraEngine.Frameworks.Nexities.Library.Components;

namespace AterraEngine.Frameworks.Nexities.Library.Entities;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public sealed class BasicEntity : NexitiesEntity<BasicEntity>, IBasicEntity {
    private const int TransformIndex = 0;
    private const int SpriteIndex = 1;
    
    public ITransformComponent Transform => GetComponent<ITransformComponent>(TransformIndex);
    public ISpriteComponent Sprite => GetComponent<ISpriteComponent>(SpriteIndex);

    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    public BasicEntity() : base(2) => PopulateComponents();
    
    protected override void PopulateComponents() {
        SetComponent<TransformComponent>(TransformIndex);
        SetComponent<SpriteComponent>(SpriteIndex);
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void Deconstruct(out ITransformComponent transform, out ISpriteComponent sprite) {
        transform = Transform;
        sprite = Sprite;
    }
}