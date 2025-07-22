// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;

namespace AterraEngine.Frameworks.Nexities.Variants;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class MoveEntitySystem : NexitiesSystem<BasicEntity>{
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override void Update(in BasicEntity entity, float delta) {
        TransformComponent transform = entity.Transform;
            
        
        float x = (Random.Shared.NextSingle() - 0.5f) * 2f; // Range: -1 to 1
        float y = (Random.Shared.NextSingle() - 0.5f) * 2f; // Range: -1 to 1
    
        transform.Position += new Vector2(x * 100f, y * 100f) * delta;
    }
}