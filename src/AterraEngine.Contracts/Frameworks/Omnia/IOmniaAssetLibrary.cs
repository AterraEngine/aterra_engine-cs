// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;

namespace AterraEngine.Frameworks.Omnia;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IOmniaAssetLibrary {
    bool TryGetInstance<T>(OmniaId omniaId, [NotNullWhen(true)] out T? asset) where T : class, IOmniaAsset;
    bool TryGetInstance<T>([NotNullWhen(true)] out T? asset) where T : class, IOmniaAsset;
    void ReturnInstance<T>(T asset) where T : class, IOmniaAsset;
}
