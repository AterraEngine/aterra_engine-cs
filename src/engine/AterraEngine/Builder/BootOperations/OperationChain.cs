// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Contracts.Builder.BootOperations;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace AterraEngine.Builder.BootOperations;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class BootOperationChain : IBootOperationChain {
    public LinkedList<Type> BootOperations { get; } 
    public HashSet<Type> BootOperationTypes { get; }
    public Type ChainVariableType { get; }

    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    private BootOperationChain(Type[] types, Type chainVariableType) {
        BootOperations = new LinkedList<Type>(types);
        BootOperationTypes = [..types];
        ChainVariableType = chainVariableType ;
    }

    public static bool TryCreate<T>(Type[] types, [NotNullWhen(true)] out IBootOperationChain? chain) where T : IChainVariables {
        chain = new BootOperationChain(types, typeof(T));
        if (chain.BootOperationTypes.Count == types.Length) return true;
        
        chain = null;
        return false;
    }

    public IEnumerator GetEnumerator() => BootOperations.GetEnumerator();
    IEnumerator<Type> IEnumerable<Type>.GetEnumerator() => BootOperations.GetEnumerator();

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
