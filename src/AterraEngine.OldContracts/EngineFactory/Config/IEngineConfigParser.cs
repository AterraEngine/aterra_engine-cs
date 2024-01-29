// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.DTO.EngineConfig;

namespace AterraEngine.OldContracts.EngineFactory.Config;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Represents a generic interface for an engine configuration parser.
/// </summary>
/// <typeparam name="T">The type of the engine configuration.</typeparam>
public interface IEngineConfigParser<T> where T:EngineConfigDto {
    
    /// <summary>
    /// Tries to deserialize an object of type T from a file located at the specified filePath.
    /// If deserialization is successful, the deserialized object is assigned to the engineConfig parameter and true is returned.
    /// If deserialization fails, the engineConfig parameter is assigned null and false is returned.
    /// </summary>
    /// <typeparam name="T">The type of object to deserialize.</typeparam>
    /// <param name="filePath">The path to the file containing the serialized object.</param>
    /// <param name="engineConfig">When this method returns, contains the deserialized object of type T if deserialization is successful; otherwise, null.</param>
    /// <returns>True if deserialization is successful; otherwise, false.</returns>
    public bool TryDeserializeFromFile(string filePath, out T? engineConfig);

    /// <summary>
    /// Attempts to serialize the provided <paramref name="engineConfig"/> object to a file located at the specified <paramref name="filePath"/>.
    /// </summary>
    /// <param name="engineConfig">The engine configuration object to serialize.</param>
    /// <param name="filePath">The path of the file to which the engine configuration object should be serialized.</param>
    /// <returns>
    /// Returns true if the serialization process succeeded and the file was successfully created or overwritten;
    /// otherwise, returns false.
    /// </returns>
    public bool TrySerializeToFile(T engineConfig, string filePath);
}