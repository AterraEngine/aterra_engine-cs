// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Contracts.ECS;
using AterraEngine.Contracts.ECS.Logic;
namespace AterraEngine.Core.ECS.Logic;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class LogicSystem<T> : EntitySystem<T>, ILogicSystem<T> where T : IEntity {
    public abstract void Process(IEntity entity, float deltaTime);
}