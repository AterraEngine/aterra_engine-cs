// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Contracts.Builder;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace AterraEngine.Builder;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class BootOperationChain : IBootOperationChain {
    public LinkedList<Type> BootOperations { get; } 
    public HashSet<Type> BootOperationTypes { get; }

    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    private BootOperationChain(Type[] types) {
        BootOperations = new LinkedList<Type>(types);
        BootOperationTypes = [..types];
    }

    public static bool TryCreate(Type[] types, [NotNullWhen(true)] out IBootOperationChain? chain) {
        chain = new BootOperationChain(types);
        if (chain.BootOperationTypes.Count == types.Length) return true;
        
        chain = null;
        return false;
    }

    public IEnumerator GetEnumerator() => BootOperations.GetEnumerator();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public bool TryAddLast(Type type) {
        if (!BootOperationTypes.Add(type)) return false;
        BootOperations.AddLast(type);
        return true;
    }
    
    public bool TryAddFirst(Type type) {
        if (!BootOperationTypes.Add(type)) return false;
        BootOperations.AddFirst(type);
        return true;
        
    }
    
    public bool TryAddLastRange<T>(T types) where T : IEnumerable<Type> {
        if (BootOperationTypes.Overlaps(types)) return false;
        foreach (Type type in types) {
            BootOperations.AddLast(type);
            BootOperationTypes.Add(type);
        }
        return true;
    }
    
    public bool TryAddFirstRange<T>(T types) where T : IEnumerable<Type>{
        if (BootOperationTypes.Overlaps(types)) return false;
        foreach (Type type in types.Reverse()) {
            BootOperations.AddFirst(type);
            BootOperationTypes.Add(type);
        }
        return true;
    }
}
