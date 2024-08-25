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
public class TextureAttribute(
    string assetId,
    CoreTags coreTags = CoreTags.Texture | CoreTags.Singleton,
    ServiceLifetime lifetime = ServiceLifetime.Singleton,
    params Type[] interfaceTypes
) : AssetAttribute(
    assetId,
    coreTags | CoreTags.Texture | CoreTags.Singleton,
    lifetime,
    interfaceTypes
);

[UsedImplicitly] public class TextureAttribute<TInterface>(string assetId,  CoreTags coreTags = CoreTags.Texture | CoreTags.Singleton, ServiceLifetime lifetime = ServiceLifetime.Singleton) : TextureAttribute(assetId, coreTags,lifetime, typeof(TInterface));
[UsedImplicitly] public class TextureAttribute<T1, T2>(string assetId,  CoreTags coreTags = CoreTags.Texture | CoreTags.Singleton, ServiceLifetime lifetime = ServiceLifetime.Singleton) : TextureAttribute(assetId, coreTags,lifetime, typeof(T1), typeof(T2));
[UsedImplicitly] public class TextureAttribute<T1, T2, T3>(string assetId, CoreTags coreTags = CoreTags.Texture | CoreTags.Singleton, ServiceLifetime lifetime = ServiceLifetime.Singleton) : TextureAttribute(assetId, coreTags,lifetime, typeof(T1), typeof(T2), typeof(T3));
[UsedImplicitly] public class TextureAttribute<T1, T2, T3, T4>(string assetId, CoreTags coreTags = CoreTags.Texture | CoreTags.Singleton, ServiceLifetime lifetime = ServiceLifetime.Singleton) : TextureAttribute(assetId, coreTags,lifetime, typeof(T1), typeof(T2), typeof(T3),typeof(T4));
[UsedImplicitly] public class TextureAttribute<T1, T2, T3, T4, T5>(string assetId, CoreTags coreTags = CoreTags.Texture | CoreTags.Singleton, ServiceLifetime lifetime = ServiceLifetime.Singleton) : TextureAttribute(assetId, coreTags,lifetime, typeof(T1), typeof(T2), typeof(T3),typeof(T4), typeof(T5));
