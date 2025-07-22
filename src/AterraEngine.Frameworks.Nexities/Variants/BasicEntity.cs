// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraEngine.Frameworks.Nexities.Variants;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class BasicEntity : NexitiesEntity {
    public TransformComponent Transform => (TransformComponent)Components[0];
    public SpriteComponent Sprite => (SpriteComponent)Components[1];

    public BasicEntity() : base(2) {
        SetComponent<TransformComponent>(0);
        SetComponent<SpriteComponent>(1);
    }
    
    public void Deconstruct(out TransformComponent transform, out SpriteComponent sprite) {
        transform = Transform;
        sprite = Sprite;
    }
}