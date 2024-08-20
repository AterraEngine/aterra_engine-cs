// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using System.Diagnostics.CodeAnalysis;

namespace AterraCore.Contracts.OmniVault.Assets;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IAssetInstanceFactory {
    bool TryCreate<T>(AssetRegistration registration, Ulid predefinedUlid, [NotNullWhen(true)] out T? assetInstance) where T : class, IAssetInstance;
    T Create<T>(AssetRegistration registration, Ulid predefinedUlid ) where T : class, IAssetInstance;
}
