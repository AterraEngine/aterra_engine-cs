// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Common.Types.Nexities;
using AterraCore.Nexities.Assets;
using JetBrains.Annotations;

namespace AterraCore.Nexities.Systems;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class SystemAttribute(
    string assetId,
    ServiceLifetimeType instanceType = ServiceLifetimeType.Singleton,
    CoreTags coreTags = CoreTags.System,
    params Type[] interfaceTypes
) : AssetAttribute(
    assetId,
    instanceType,
    coreTags | CoreTags.System,
    interfaceTypes
);

[UsedImplicitly]
public class SystemAttribute<TInterface>( string assetId, ServiceLifetimeType instanceType = ServiceLifetimeType.Multiple, CoreTags coreTags = CoreTags.Entity ) : SystemAttribute(assetId, instanceType, coreTags, typeof(TInterface));
