// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Contracts.Builder;

namespace AterraEngine.Builder;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// The BootOperationBuilder class is responsible for managing a sequence of boot operations.
/// </summary>
/// <remarks>
/// This class provides methods to register boot operations and organize their order of execution
/// by specifying whether an operation should be placed before or after another operation,
/// based on a provided predicate.
/// </remarks>
public class BootOperationBuilder : IBootOperationBuilder {
    /// <summary>
    /// Gets the linked list of boot operation types.
    /// </summary>
    private LinkedList<Type> BootOperations { get; } = [];
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// <inheritdoc/>
    public IBootOperationBuilder Register<T>() where T : IBootOperation {
        BootOperations.AddLast(typeof(T));
        return this;
    }

    /// <inheritdoc/>
    public IBootOperationBuilder RegisterAfter<T>(Func<Type, bool> predicate) where T : IBootOperation {
    LinkedListNode<Type>? current = BootOperations.First;
        while (current != null) {
            if (predicate(current.Value)) {
                BootOperations.AddAfter(current, typeof(T));
                return this;
            }
            current = current.Next;
        }

        BootOperations.AddLast(typeof(T));
        return this;
    }

    /// <inheritdoc/>
    public IBootOperationBuilder RegisterBefore<T>(Func<Type, bool> predicate) where T : IBootOperation {
        LinkedListNode<Type>? current = BootOperations.First;
        while (current != null) {
            if (predicate(current.Value)) {
                BootOperations.AddBefore(current, typeof(T));
                return this;
            }
            current = current.Next;
        }
        BootOperations.AddLast(typeof(T));
        return this;
    }
}
