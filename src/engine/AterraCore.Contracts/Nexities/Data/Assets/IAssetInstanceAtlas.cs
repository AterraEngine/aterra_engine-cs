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
    bool TryCreateInstance<T>(AssetId assetId, [NotNullWhen(true)] out T? instance, Guid? predefinedGuid = null) where T : IAssetInstance;
    public bool TryCreateInstance<T>([NotNullWhen(true)] out T? instance) where T : IAssetInstance;
    public bool TryCreateInstance(Type type, [NotNullWhen(true)] out IAssetInstance? instance);
    bool TryGetInstance<T>(Guid instanceId, [NotNullWhen(true)] out T? instance) where T : IAssetInstance;
}
