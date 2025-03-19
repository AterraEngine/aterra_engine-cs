// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;

namespace AterraEngine.Frameworks.Omnia;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IOmniaRegistration {
    OmniaId OmniaId { get; }
    Type AssetType { get; }
    bool UsesPool { get; }

    void ReturnToPool<TAsset>(TAsset instance);
    TAsset CreateInstance<TAsset>(IScopedProvider scopedProvider) where TAsset : IOmniaAsset;
    bool TryCreateInstance<TAsset>(IScopedProvider scopedProvider, [NotNullWhen(true)] out TAsset? asset) where TAsset : IOmniaAsset;
    IOmniaRegistrationKey GetLookupKey();
}
