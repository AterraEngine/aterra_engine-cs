// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace AterraCore.Attributes;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class SystemAttribute(
    string assetId,
    CoreTags coreTags = CoreTags.System,
    ServiceLifetime lifetime = ServiceLifetime.Transient,
    params Type[] interfaceTypes
) : AssetAttribute(
    assetId,
    coreTags | CoreTags.System,
    lifetime,
    interfaceTypes
);

[UsedImplicitly]
public class SystemAttribute<TInterface>(string assetId, CoreTags coreTags = CoreTags.Entity, ServiceLifetime lifetime = ServiceLifetime.Transient) : SystemAttribute(assetId, coreTags, lifetime, typeof(TInterface));
