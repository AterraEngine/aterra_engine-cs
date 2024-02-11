// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Contracts.Core.ECSFramework;
namespace AterraEngine.Core.ECSFramework;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class System<T> : ISystem<T> where T : class, IEntity {
    public IEnumerable<T> Filter(IEnumerable<IEntity> entities) => entities.OfType<T>();
    
    public abstract void Process(T entity);
}