﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraCore.Common.Attributes;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class LevelAttribute(
    string assetId,
    CoreTags coreTags = CoreTags.Level | CoreTags.Singleton,
    ServiceLifetime lifetime = ServiceLifetime.Singleton,
    params Type[] interfaceTypes
) : AssetAttribute(
    assetId,
    coreTags,
    lifetime,
    interfaceTypes
);

[UsedImplicitly] [ExcludeFromCodeCoverage] public class LevelAttribute<TInterface>(string assetId, CoreTags coreTags = CoreTags.Level, ServiceLifetime lifetime = ServiceLifetime.Singleton) : LevelAttribute(assetId, coreTags, lifetime, typeof(TInterface));

[UsedImplicitly] [ExcludeFromCodeCoverage] public class LevelAttribute<T1, T2>(string assetId, CoreTags coreTags = CoreTags.Level, ServiceLifetime lifetime = ServiceLifetime.Singleton) : LevelAttribute(assetId, coreTags, lifetime, typeof(T1), typeof(T2));

[UsedImplicitly] [ExcludeFromCodeCoverage] public class LevelAttribute<T1, T2, T3>(string assetId, CoreTags coreTags = CoreTags.Level, ServiceLifetime lifetime = ServiceLifetime.Singleton) : LevelAttribute(assetId, coreTags, lifetime, typeof(T1), typeof(T2), typeof(T3));

[UsedImplicitly] [ExcludeFromCodeCoverage] public class LevelAttribute<T1, T2, T3, T4>(string assetId, CoreTags coreTags = CoreTags.Level, ServiceLifetime lifetime = ServiceLifetime.Singleton) : LevelAttribute(assetId, coreTags, lifetime, typeof(T1), typeof(T2), typeof(T3), typeof(T4));

[UsedImplicitly] [ExcludeFromCodeCoverage] public class LevelAttribute<T1, T2, T3, T4, T5>(string assetId, CoreTags coreTags = CoreTags.Level, ServiceLifetime lifetime = ServiceLifetime.Singleton) : LevelAttribute(assetId, coreTags, lifetime, typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5));

[UsedImplicitly] [ExcludeFromCodeCoverage] public class LevelAttribute<T1, T2, T3, T4, T5, T6>(string assetId, CoreTags coreTags = CoreTags.Level, ServiceLifetime lifetime = ServiceLifetime.Singleton) : LevelAttribute(assetId, coreTags, lifetime, typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6));

[UsedImplicitly] [ExcludeFromCodeCoverage] public class LevelAttribute<T1, T2, T3, T4, T5, T6, T7>(string assetId, CoreTags coreTags = CoreTags.Level, ServiceLifetime lifetime = ServiceLifetime.Singleton) : LevelAttribute(assetId, coreTags, lifetime, typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7));

[UsedImplicitly] [ExcludeFromCodeCoverage] public class LevelAttribute<T1, T2, T3, T4, T5, T6, T7, T8>(string assetId, CoreTags coreTags = CoreTags.Level, ServiceLifetime lifetime = ServiceLifetime.Singleton) : LevelAttribute(assetId, coreTags, lifetime, typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8));

[UsedImplicitly] [ExcludeFromCodeCoverage] public class LevelAttribute<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string assetId, CoreTags coreTags = CoreTags.Level, ServiceLifetime lifetime = ServiceLifetime.Singleton) : LevelAttribute(assetId, coreTags, lifetime, typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9));

[UsedImplicitly] [ExcludeFromCodeCoverage] public class LevelAttribute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string assetId, CoreTags coreTags = CoreTags.Level, ServiceLifetime lifetime = ServiceLifetime.Singleton) : LevelAttribute(assetId, coreTags, lifetime, typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10));

[UsedImplicitly] [ExcludeFromCodeCoverage] public class LevelAttribute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string assetId, CoreTags coreTags = CoreTags.Level, ServiceLifetime lifetime = ServiceLifetime.Singleton) : LevelAttribute(assetId, coreTags, lifetime, typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11));

[UsedImplicitly] [ExcludeFromCodeCoverage] public class LevelAttribute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string assetId, CoreTags coreTags = CoreTags.Level, ServiceLifetime lifetime = ServiceLifetime.Singleton) : LevelAttribute(assetId, coreTags, lifetime, typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12));

[UsedImplicitly] [ExcludeFromCodeCoverage] public class LevelAttribute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(string assetId, CoreTags coreTags = CoreTags.Level, ServiceLifetime lifetime = ServiceLifetime.Singleton) : LevelAttribute(assetId, coreTags, lifetime, typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13));

[UsedImplicitly] [ExcludeFromCodeCoverage] public class LevelAttribute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(string assetId, CoreTags coreTags = CoreTags.Level, ServiceLifetime lifetime = ServiceLifetime.Singleton) : LevelAttribute(assetId, coreTags, lifetime, typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13), typeof(T14));

[UsedImplicitly] [ExcludeFromCodeCoverage] public class LevelAttribute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(string assetId, CoreTags coreTags = CoreTags.Level, ServiceLifetime lifetime = ServiceLifetime.Singleton) : LevelAttribute(assetId, coreTags, lifetime, typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13), typeof(T14), typeof(T15));

[UsedImplicitly] [ExcludeFromCodeCoverage] public class LevelAttribute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(string assetId, CoreTags coreTags = CoreTags.Level, ServiceLifetime lifetime = ServiceLifetime.Singleton) : LevelAttribute(assetId, coreTags, lifetime, typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13), typeof(T14), typeof(T15), typeof(T16));