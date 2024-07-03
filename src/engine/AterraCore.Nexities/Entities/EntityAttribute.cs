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
    ServiceLifetimeType instanceType = ServiceLifetimeType.Multiple,
    CoreTags coreTags = CoreTags.Entity,
    params Type[] interfaceTypes
) : AssetAttribute(
    assetId,
    instanceType,
    coreTags | CoreTags.Entity,
    interfaceTypes
);

[UsedImplicitly] public class EntityAttribute<TInterface>(string assetId, ServiceLifetimeType instanceType = ServiceLifetimeType.Multiple, CoreTags coreTags = CoreTags.Component) : EntityAttribute(assetId, instanceType, coreTags, typeof(TInterface));
[UsedImplicitly] public class EntityAttribute<T1, T2>(string assetId, ServiceLifetimeType instanceType = ServiceLifetimeType.Multiple, CoreTags coreTags = CoreTags.Component) : EntityAttribute(assetId, instanceType, coreTags, typeof(T1), typeof(T2));
[UsedImplicitly] public class EntityAttribute<T1, T2, T3>(string assetId, ServiceLifetimeType instanceType = ServiceLifetimeType.Multiple, CoreTags coreTags = CoreTags.Component) : EntityAttribute(assetId, instanceType, coreTags, typeof(T1), typeof(T2), typeof(T3));
