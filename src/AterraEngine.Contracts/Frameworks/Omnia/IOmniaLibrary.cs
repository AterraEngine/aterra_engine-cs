// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;

namespace AterraEngine.Frameworks.Omnia;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IOmniaLibrary {
    TAsset GetByType<TAsset>() where TAsset : class, IOmniaAsset;
    TAsset GetByQuery<TAsset>(Guid instanceId);
    bool TryGetFromPool<T>([NotNullWhen(true)] out T? output, Func<IOmniaLibrary, T> createFactory);
}
