// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Contracts.Assets;
using AterraEngine.Contracts.ECS;
using AterraEngine.Contracts.ECS.Logic;
namespace AterraEngine.Core.ECS;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class EntitySystemManager<T>: IEntitySystemManager<T> where T: IEntitySystem<IEntity> {
    public List<T> EntitySystems { get; init; } = [];

    public bool TryAdd(T system) {
        // TODO ... well actually try haha
        EntitySystems.Add(system);
        return true;
    }

    protected void ForEachEntity(IEnumerable<IEntity> entities, Action<T, IEntity> action) {
        IEntity[] enumerable = entities as IEntity[] ?? entities.ToArray();
        if (enumerable.Length == 0) return ;

        foreach (T system in EntitySystems) {
            foreach (IEntity entity in enumerable) {
                if (!system.ComponentTypes.All(t => entity.CheckForComponent(t))) continue;
                
                action(system, entity);
            }
        }
    }
}