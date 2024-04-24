// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;

namespace AterraCore.Contracts.Config;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IConfigXmlParser<T> {
    public bool TryDeserializeFromFile(string filePath, [NotNullWhen(true)] out T? engineConfig);
    public bool TrySerializeToFile(T engineConfig, string filePath);
    public bool TrySerializeFromBytes(byte[] bytes,
        [NotNullWhen(true)] out T? config);
}