// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.OmniVault.Assets;
using System.Reflection;

namespace AterraCore.AssetVault;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class AssetInstance : IAssetInstance {
    public Ulid InstanceId  { get; set; }
    public AssetId AssetId { get; set; }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public virtual void OnCreate(Ulid instanceId, AssetId assetId) {
        InstanceId = instanceId;
        AssetId = assetId;
    }
}