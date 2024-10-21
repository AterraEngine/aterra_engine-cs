// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraEngine.Contracts.Builder.BootOperations;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IBootOperationChain : IEnumerable<Type> {
    LinkedList<Type> BootOperations { get; } 
    HashSet<Type> BootOperationTypes { get; }
    Type ChainVariableType { get; }

    bool TryAddLast(Type type);
    bool TryAddFirst(Type type);
    bool TryAddLastRange<T>(T types) where T : IEnumerable<Type>;
    bool TryAddFirstRange<T>(T types) where T : IEnumerable<Type>;
}
