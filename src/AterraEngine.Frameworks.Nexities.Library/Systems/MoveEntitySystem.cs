// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;
using AterraEngine.Frameworks.Nexities.Library.Components;
using AterraEngine.Frameworks.Nexities.Library.Entities;
using CodeOfChaos.Extensions.DependencyInjection;

namespace AterraEngine.Frameworks.Nexities.Library.Systems;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableSingleton<MoveEntitySystem>]
public class MoveEntitySystem : NexitiesSystem<IBasicEntity>{
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override void Update(in IBasicEntity entity, float delta) {
        ITransformComponent transform = entity.Transform;
        
        float x = (Random.Shared.NextSingle() - 0.5f) * 2f; // Range: -1 to 1
        float y = (Random.Shared.NextSingle() - 0.5f) * 2f; // Range: -1 to 1
    
        transform.Position += new Vector2(x * 100f, y * 100f) * delta;
    }
}