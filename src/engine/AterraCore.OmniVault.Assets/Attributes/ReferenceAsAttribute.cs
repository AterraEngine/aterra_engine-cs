// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.OmniVault.Assets.Attributes;
using JetBrains.Annotations;

namespace AterraCore.OmniVault.Assets.Attributes;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true)]
[UsedImplicitly]
public class ReferenceAsAttribute(string assetId) : IReferenceAsAttribute {
    public override AssetId AssetId { get; } = assetId;
}
