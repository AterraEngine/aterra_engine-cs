// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Nexities.Systems;
using Microsoft.Extensions.ObjectPool;

namespace AterraCore.Contracts.PoolCorps;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface INexitiesSystemPools {
    ObjectPool<List<INexitiesSystem>> ListPool { get; }
    ObjectPool<LinkedList<INexitiesSystem>> LinkedListPool { get; }
}
