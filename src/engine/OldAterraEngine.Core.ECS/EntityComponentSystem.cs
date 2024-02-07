// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using OldAterraEngine.Contracts.ECS;
namespace OldAterraEngine.Core.ECS;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class EntityComponentSystem<T> : IEntityComponentSystem where T : IEntity {
    public T CastToEntity(IEntity entity) => (T)entity;
    public bool CheckEntity(object? entity) => entity is T;
    
    public abstract void Update(IEntity entity);
}