// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Common.Types.Nexities;
using AterraCore.Nexities.Assets;

namespace AterraCore.Nexities.Entities;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class EntityAttribute(
    string assetId,
    ServiceLifetimeType instanceType = ServiceLifetimeType.Multiple,
    CoreTags coreTags = CoreTags.Entity,
    params Type[] interfaceTypes
) : AssetAttribute(
    assetId,
    instanceType,
    coreTags | CoreTags.Entity,
    interfaceTypes
);

public class EntityAttribute<TInterface>( string assetId, ServiceLifetimeType instanceType = ServiceLifetimeType.Multiple, CoreTags coreTags = CoreTags.Entity ) : EntityAttribute(assetId, instanceType, coreTags, typeof(TInterface));
