// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Data;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace AterraCore.Attributes;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class EntityAttribute(
    string assetId,
    CoreTags coreTags = CoreTags.Entity,
    ServiceLifetime lifetime = ServiceLifetime.Transient,
    params Type[] interfaceTypes
) : AssetAttribute(
    assetId,
    coreTags | CoreTags.Entity,
    lifetime,
    interfaceTypes
);

[UsedImplicitly] public class EntityAttribute<TInterface>(string assetId, CoreTags coreTags = CoreTags.Entity, ServiceLifetime lifetime = ServiceLifetime.Transient) : EntityAttribute(assetId, coreTags, lifetime, typeof(TInterface));

[UsedImplicitly] public class EntityAttribute<T1, T2>(string assetId, CoreTags coreTags = CoreTags.Entity, ServiceLifetime lifetime = ServiceLifetime.Transient) : EntityAttribute(assetId, coreTags, lifetime, typeof(T1), typeof(T2));

[UsedImplicitly] public class EntityAttribute<T1, T2, T3>(string assetId, CoreTags coreTags = CoreTags.Entity, ServiceLifetime lifetime = ServiceLifetime.Transient) : EntityAttribute(assetId, coreTags, lifetime, typeof(T1), typeof(T2), typeof(T3));

[UsedImplicitly] public class EntityAttribute<T1, T2, T3, T4>(string assetId, CoreTags coreTags = CoreTags.Entity, ServiceLifetime lifetime = ServiceLifetime.Transient) : EntityAttribute(assetId, coreTags, lifetime, typeof(T1), typeof(T2), typeof(T3), typeof(T4));

[UsedImplicitly] public class EntityAttribute<T1, T2, T3, T4, T5>(string assetId, CoreTags coreTags = CoreTags.Entity, ServiceLifetime lifetime = ServiceLifetime.Transient) : EntityAttribute(assetId, coreTags, lifetime, typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5));
