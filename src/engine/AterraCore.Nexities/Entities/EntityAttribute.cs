// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using AterraCore.OmniVault.Assets.Attributes;
using JetBrains.Annotations;

namespace AterraCore.Nexities.Entities;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class EntityAttribute(
    string assetId,
    CoreTags coreTags = CoreTags.Entity,
    params Type[] interfaceTypes
) : AssetAttribute(
    assetId,
    coreTags | CoreTags.Entity,
    interfaceTypes
);

[UsedImplicitly] public class EntityAttribute<TInterface>(string assetId,  CoreTags coreTags = CoreTags.Entity) : EntityAttribute(assetId, coreTags, typeof(TInterface));
[UsedImplicitly] public class EntityAttribute<T1, T2>(string assetId,  CoreTags coreTags = CoreTags.Entity) : EntityAttribute(assetId, coreTags, typeof(T1), typeof(T2));
[UsedImplicitly] public class EntityAttribute<T1, T2, T3>(string assetId, CoreTags coreTags = CoreTags.Entity) : EntityAttribute(assetId, coreTags, typeof(T1), typeof(T2), typeof(T3));
[UsedImplicitly] public class EntityAttribute<T1, T2, T3, T4>(string assetId, CoreTags coreTags = CoreTags.Entity) : EntityAttribute(assetId, coreTags, typeof(T1), typeof(T2), typeof(T3),typeof(T4));
[UsedImplicitly] public class EntityAttribute<T1, T2, T3, T4, T5>(string assetId, CoreTags coreTags = CoreTags.Entity) : EntityAttribute(assetId, coreTags, typeof(T1), typeof(T2), typeof(T3),typeof(T4), typeof(T5));