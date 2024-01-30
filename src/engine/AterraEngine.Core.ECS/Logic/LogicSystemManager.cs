// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Contracts.ECS;
using AterraEngine.Contracts.ECS.Logic;
namespace AterraEngine.Core.ECS.Logic;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class LogicSystemManager : EntitySystemManager<ILogicSystem<IEntity>>, ILogicSystemManager {
    public void UpdateEntities(IEnumerable<IEntity> entities, float deltaTime) {
        ForEachEntity(
            entities,
            (system, entity) => system.Process(entity, deltaTime)
        );
    }
}