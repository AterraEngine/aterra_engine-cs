// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraEngine.Contracts.Builder;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IBootOperationBuilder {
    /// <summary>
    /// Registers a boot operation of the specified type.
    /// </summary>
    /// <typeparam name="T">The type of the boot operation to register.</typeparam>
    /// <return>
    /// An instance of <see cref="IBootOperationBuilder"/> to allow method chaining.
    /// </return>
    IBootOperationBuilder Register<T>() where T : IBootOperation;
    
    /// <summary>
    /// Registers a boot operation of type <typeparamref name="T"/> to be executed after the
    /// existing boot operation that matches the specified predicate.
    /// </summary>
    /// <typeparam name="T">The type of the boot operation to register.</typeparam>
    /// <param name="predicate">A function that takes a <see cref="Type"/> and returns a boolean
    /// indicating whether the current boot operation matches the condition.</param>
    /// <returns>The current instance of <see cref="IBootOperationBuilder"/> for method chaining.</returns>
    IBootOperationBuilder RegisterAfter<T>(Func<Type, bool> predicate) where T : IBootOperation;
    
    /// <summary>
    /// Registers a boot operation to be executed before the specified operation.
    /// </summary>
    /// <typeparam name="T">The type of boot operation to register.</typeparam>
    /// <param name="predicate">A function that determines which existing boot operation to register the new operation before.</param>
    /// <returns>The current instance of <see cref="IBootOperationBuilder"/>.</returns>
    IBootOperationBuilder RegisterBefore<T>(Func<Type, bool> predicate) where T : IBootOperation;
    
    
}
