// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Collections;

namespace AterraEngine.Contracts.Builder;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IBootOperationChain : IEnumerable {
    LinkedList<Type> BootOperations { get; } 
    HashSet<Type> BootOperationTypes { get; }

    bool TryAddLast(Type type);
    bool TryAddFirst(Type type);
    bool TryAddLastRange<T>(T types) where T : IEnumerable<Type>;
    bool TryAddFirstRange<T>(T types) where T : IEnumerable<Type>;
}
