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
public class LevelAttribute(
    string assetId,
    CoreTags coreTags = CoreTags.Level,
    ServiceLifetime lifetime = ServiceLifetime.Transient,
    params Type[] interfaceTypes
) : AssetAttribute(
    assetId,
    coreTags | CoreTags.Level,
    lifetime,
    interfaceTypes
);

[UsedImplicitly] public class LevelAttribute<TInterface>(string assetId,  CoreTags coreTags = CoreTags.Level, ServiceLifetime lifetime = ServiceLifetime.Transient) : LevelAttribute(assetId, coreTags, lifetime, typeof(TInterface));
[UsedImplicitly] public class LevelAttribute<T1, T2>(string assetId,  CoreTags coreTags = CoreTags.Level, ServiceLifetime lifetime = ServiceLifetime.Transient) : LevelAttribute(assetId, coreTags, lifetime, typeof(T1), typeof(T2));
[UsedImplicitly] public class LevelAttribute<T1, T2, T3>(string assetId, CoreTags coreTags = CoreTags.Level, ServiceLifetime lifetime = ServiceLifetime.Transient) : LevelAttribute(assetId, coreTags, lifetime, typeof(T1), typeof(T2), typeof(T3));
[UsedImplicitly] public class LevelAttribute<T1, T2, T3, T4>(string assetId, CoreTags coreTags = CoreTags.Level, ServiceLifetime lifetime = ServiceLifetime.Transient) : LevelAttribute(assetId, coreTags, lifetime, typeof(T1), typeof(T2), typeof(T3),typeof(T4));
[UsedImplicitly] public class LevelAttribute<T1, T2, T3, T4, T5>(string assetId, CoreTags coreTags = CoreTags.Level, ServiceLifetime lifetime = ServiceLifetime.Transient) : LevelAttribute(assetId, coreTags, lifetime, typeof(T1), typeof(T2), typeof(T3),typeof(T4), typeof(T5));