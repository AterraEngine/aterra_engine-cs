// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using Microsoft.Extensions.DependencyInjection;

namespace AterraCore.Contracts.OmniVault.Assets;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
// ReSharper disable once InconsistentNaming
public abstract class IAssetAttribute : Attribute {
    public abstract AssetId AssetId { get; }
    public abstract CoreTags CoreTags { get; }
    public abstract Type[] InterfaceTypes { get; }
    public virtual ServiceLifetime Lifetime { get; } = ServiceLifetime.Transient;
}
