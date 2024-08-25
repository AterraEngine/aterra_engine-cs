// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Common.Data;
using AterraCore.Common.Types.Nexities;
using System.Diagnostics.CodeAnalysis;

namespace AterraCore.Contracts.OmniVault.Assets;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IAssetInstanceAtlas {
    public int TotalCount { get; }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    bool TryCreate<T>(AssetId assetId, [NotNullWhen(true)] out T? instance, Ulid? predefinedUlid = null) where T : class, IAssetInstance;
    bool TryCreate<T>([NotNullWhen(true)] out T? instance, Ulid? predefinedUlid = null) where T : class, IAssetInstance;
    bool TryCreate<T>(Type type, [NotNullWhen(true)] out T? instance, Ulid? predefinedUlid = null) where T : class, IAssetInstance;
    
    bool TryGet<T>(Ulid instanceId, [NotNullWhen(true)] out T? instance) where T : class, IAssetInstance;
    bool TryGetSingleton<T>(AssetId assetId, [NotNullWhen(true)] out T? instance) where T : class, IAssetInstance;

    bool TryGetOrCreate<T>(Type type, Ulid? ulid, [NotNullWhen(true)] out T? instance) where T : class, IAssetInstance;
    bool TryGetOrCreate<T>(AssetId assetId, Ulid? ulid, [NotNullWhen(true)] out T? instance) where T : class, IAssetInstance;
    
    bool TryGetOrCreateSingleton<T>(AssetId assetId, [NotNullWhen(true)] out T? instance) where T : class, IAssetInstance;
    
    T GetOrCreate<T>(Type type, Ulid? ulid = null) where T : class, IAssetInstance;
    T GetOrCreate<T>(AssetId assetId, Ulid? ulid = null) where T : class, IAssetInstance;
    
    IEnumerable<T> OfType<T>() where T : class, IAssetInstance;
    IEnumerable<T> OfTag<T>(CoreTags level) where T : class, IAssetInstance;
}
