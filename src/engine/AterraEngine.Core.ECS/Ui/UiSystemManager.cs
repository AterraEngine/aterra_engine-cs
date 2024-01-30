// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;
using AterraEngine.Contracts.ECS;
using AterraEngine.Contracts.ECS.Logic;
using AterraEngine.Contracts.ECS.Ui;
namespace AterraEngine.Core.ECS.Ui;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class UiSystemManager : EntitySystemManager<IUiSystem<IEntity>>, IUiSystemManager {
    public void UpdateEntities(IEnumerable<IEntity> entities, float deltaTime, Vector2 worldToScreen) {
        ForEachEntity(
            entities,
            (system, entity) => system.Process(entity, deltaTime, worldToScreen)
        );
    }
}