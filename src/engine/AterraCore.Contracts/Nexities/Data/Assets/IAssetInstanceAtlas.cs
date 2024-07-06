// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Common.Types.Nexities;
using System.Diagnostics.CodeAnalysis;

namespace AterraCore.Contracts.Nexities.Data.Assets;
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

    bool TryGetOrCreate<T>(Type type, Guid? guid, [NotNullWhen(true)] out T? instance) where T : class, IAssetInstance;
    bool TryGetOrCreate<T>(AssetId assetId, Guid? guid, [NotNullWhen(true)] out T? instance) where T : class, IAssetInstance;
}
