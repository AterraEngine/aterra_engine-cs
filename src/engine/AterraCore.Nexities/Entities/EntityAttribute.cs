// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Common.Types.Nexities;
using AterraCore.Nexities.Assets;
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

[UsedImplicitly] public class EntityAttribute<TInterface>(string assetId,  CoreTags coreTags = CoreTags.Component) : EntityAttribute(assetId, coreTags, typeof(TInterface));
[UsedImplicitly] public class EntityAttribute<T1, T2>(string assetId,  CoreTags coreTags = CoreTags.Component) : EntityAttribute(assetId, coreTags, typeof(T1), typeof(T2));
[UsedImplicitly] public class EntityAttribute<T1, T2, T3>(string assetId, CoreTags coreTags = CoreTags.Component) : EntityAttribute(assetId, coreTags, typeof(T1), typeof(T2), typeof(T3));
