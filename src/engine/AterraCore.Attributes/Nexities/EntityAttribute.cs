﻿// ---------------------------------------------------------------------------------------------------------------------
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
[UsedImplicitly] public class EntityAttribute<T1, T2, T3, T4, T5, T6>(string assetId, CoreTags coreTags = CoreTags.Entity, ServiceLifetime lifetime = ServiceLifetime.Transient) : EntityAttribute(assetId, coreTags, lifetime, typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6));
[UsedImplicitly] public class EntityAttribute<T1, T2, T3, T4, T5, T6, T7>(string assetId, CoreTags coreTags = CoreTags.Entity, ServiceLifetime lifetime = ServiceLifetime.Transient) : EntityAttribute(assetId, coreTags, lifetime, typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7));
[UsedImplicitly] public class EntityAttribute<T1, T2, T3, T4, T5, T6, T7, T8>(string assetId, CoreTags coreTags = CoreTags.Entity, ServiceLifetime lifetime = ServiceLifetime.Transient) : EntityAttribute(assetId, coreTags, lifetime, typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8));
[UsedImplicitly] public class EntityAttribute<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string assetId, CoreTags coreTags = CoreTags.Entity, ServiceLifetime lifetime = ServiceLifetime.Transient) : EntityAttribute(assetId, coreTags, lifetime, typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9));
[UsedImplicitly] public class EntityAttribute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string assetId, CoreTags coreTags = CoreTags.Entity, ServiceLifetime lifetime = ServiceLifetime.Transient) : EntityAttribute(assetId, coreTags, lifetime, typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10));
[UsedImplicitly] public class EntityAttribute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string assetId, CoreTags coreTags = CoreTags.Entity, ServiceLifetime lifetime = ServiceLifetime.Transient) : EntityAttribute(assetId, coreTags, lifetime, typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11));
[UsedImplicitly] public class EntityAttribute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string assetId, CoreTags coreTags = CoreTags.Entity, ServiceLifetime lifetime = ServiceLifetime.Transient) : EntityAttribute(assetId, coreTags, lifetime, typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12));
[UsedImplicitly] public class EntityAttribute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(string assetId, CoreTags coreTags = CoreTags.Entity, ServiceLifetime lifetime = ServiceLifetime.Transient) : EntityAttribute(assetId, coreTags, lifetime, typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13));
[UsedImplicitly] public class EntityAttribute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(string assetId, CoreTags coreTags = CoreTags.Entity, ServiceLifetime lifetime = ServiceLifetime.Transient) : EntityAttribute(assetId, coreTags, lifetime, typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13), typeof(T14));
[UsedImplicitly] public class EntityAttribute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(string assetId, CoreTags coreTags = CoreTags.Entity, ServiceLifetime lifetime = ServiceLifetime.Transient) : EntityAttribute(assetId, coreTags, lifetime, typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13), typeof(T14), typeof(T15));
[UsedImplicitly] public class EntityAttribute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(string assetId, CoreTags coreTags = CoreTags.Entity, ServiceLifetime lifetime = ServiceLifetime.Transient) : EntityAttribute(assetId, coreTags, lifetime, typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13), typeof(T14), typeof(T15), typeof(T16));
