// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;
using AterraEngine.Contracts.ECS.Ui;
namespace AterraEngine.Contracts.ECS.Logic;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IUiSystemManager : IEntitySystemManager<IUiSystem<IEntity>> {
    public void UpdateEntities(IEnumerable<IEntity> entities, float deltatime, Vector2 worldToScreen);
}