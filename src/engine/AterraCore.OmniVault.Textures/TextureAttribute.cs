// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using AterraCore.OmniVault.Assets.Attributes;
using JetBrains.Annotations;

namespace AterraCore.OmniVault.Textures;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class TextureAttribute(
    string assetId,
    CoreTags coreTags = CoreTags.Texture | CoreTags.Singleton,
    params Type[] interfaceTypes
) : AssetAttribute(
    assetId,
    coreTags | CoreTags.Texture | CoreTags.Singleton,
    interfaceTypes
);

[UsedImplicitly] public class TextureAttribute<TInterface>(string assetId,  CoreTags coreTags = CoreTags.Texture | CoreTags.Singleton) : TextureAttribute(assetId, coreTags, typeof(TInterface));
[UsedImplicitly] public class TextureAttribute<T1, T2>(string assetId,  CoreTags coreTags = CoreTags.Texture | CoreTags.Singleton) : TextureAttribute(assetId, coreTags, typeof(T1), typeof(T2));
[UsedImplicitly] public class TextureAttribute<T1, T2, T3>(string assetId, CoreTags coreTags = CoreTags.Texture | CoreTags.Singleton) : TextureAttribute(assetId, coreTags, typeof(T1), typeof(T2), typeof(T3));
[UsedImplicitly] public class TextureAttribute<T1, T2, T3, T4>(string assetId, CoreTags coreTags = CoreTags.Texture | CoreTags.Singleton) : TextureAttribute(assetId, coreTags, typeof(T1), typeof(T2), typeof(T3),typeof(T4));
[UsedImplicitly] public class TextureAttribute<T1, T2, T3, T4, T5>(string assetId, CoreTags coreTags = CoreTags.Texture | CoreTags.Singleton) : TextureAttribute(assetId, coreTags, typeof(T1), typeof(T2), typeof(T3),typeof(T4), typeof(T5));
