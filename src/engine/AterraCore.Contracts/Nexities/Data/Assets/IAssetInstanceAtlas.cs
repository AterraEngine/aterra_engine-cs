// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

namespace AterraCore.Contracts.Nexities.Data.Assets;

using Common.Types.Nexities;
using System.Diagnostics.CodeAnalysis;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public interface IAssetInstanceAtlas {
    public int TotalCount { get; }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    bool TryCreateInstance<T>(AssetId assetId, [NotNullWhen(true)] out T? instance,Guid? predefinedGuid = null) where T : IAssetInstance;
    bool TryGetInstance<T>(Guid instanceId, [NotNullWhen(true)] out T? instance) where T : IAssetInstance;
}