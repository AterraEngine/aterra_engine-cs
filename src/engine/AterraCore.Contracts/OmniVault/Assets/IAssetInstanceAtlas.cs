// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

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
    bool TryCreate<T>(AssetId assetId, [NotNullWhen(true)] out T? instance, Guid? predefinedGuid = null) where T : class, IAssetInstance;
    bool TryCreate<T>([NotNullWhen(true)] out T? instance, Guid? predefinedGuid = null) where T : class, IAssetInstance;
    bool TryCreate<T>(Type type, [NotNullWhen(true)] out T? instance, Guid? predefinedGuid = null) where T : class, IAssetInstance;
    
    bool TryGet<T>(Guid instanceId, [NotNullWhen(true)] out T? instance) where T : class, IAssetInstance;
    bool TryGetSingleton<T>(AssetId assetId, [NotNullWhen(true)] out T? instance) where T : class, IAssetInstance;

    bool TryGetOrCreate<T>(Type type, Guid? guid, [NotNullWhen(true)] out T? instance) where T : class, IAssetInstance;
    bool TryGetOrCreate<T>(AssetId assetId, Guid? guid, [NotNullWhen(true)] out T? instance) where T : class, IAssetInstance;
    
    bool TryGetOrCreateSingleton<T>(AssetId assetId, [NotNullWhen(true)] out T? instance) where T : class, IAssetInstance;
    
    T GetOrCreate<T>(Type type, Guid? guid = null) where T : class, IAssetInstance;
    T GetOrCreate<T>(AssetId assetId, Guid? guid = null) where T : class, IAssetInstance;
    
    IEnumerable<T> OfType<T>() where T : class, IAssetInstance;
}
