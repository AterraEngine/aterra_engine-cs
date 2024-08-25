// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Data;
using AterraCore.Common.Types.Nexities;
using Microsoft.Extensions.DependencyInjection;

namespace AterraCore.Attributes;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[AttributeUsage(AttributeTargets.Class)]
public class AssetAttribute(
    string assetId,
    CoreTags coreTags = CoreTags.Asset,
    ServiceLifetime lifetime = ServiceLifetime.Transient,
    params Type[] interfaceTypes
    
) : Attribute {
    public AssetId AssetId { get; }= assetId;
    public CoreTags CoreTags { get; } = coreTags;
    public Type[] InterfaceTypes { get; } = interfaceTypes;
    public ServiceLifetime Lifetime { get; } = lifetime ;
}
