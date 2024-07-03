// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Common.Types.Nexities;
using AterraCore.Nexities.Assets;
using JetBrains.Annotations;

namespace AterraCore.Nexities.Components;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class ComponentAttribute(
    string assetId,
    ServiceLifetimeType instanceType = ServiceLifetimeType.Multiple,
    CoreTags coreTags = CoreTags.Component,
    params Type[] @interface) : AssetAttribute(
assetId,
instanceType,
coreTags | CoreTags.Component,
@interface
);

[UsedImplicitly] public class ComponentAttribute<TInterface>(string assetId, ServiceLifetimeType instanceType = ServiceLifetimeType.Multiple, CoreTags coreTags = CoreTags.Component) : ComponentAttribute(assetId, instanceType, coreTags, typeof(TInterface));
[UsedImplicitly] public class ComponentAttribute<T1, T2>(string assetId, ServiceLifetimeType instanceType = ServiceLifetimeType.Multiple, CoreTags coreTags = CoreTags.Component) : ComponentAttribute(assetId, instanceType, coreTags, typeof(T1), typeof(T2));
[UsedImplicitly] public class ComponentAttribute<T1, T2, T3>(string assetId, ServiceLifetimeType instanceType = ServiceLifetimeType.Multiple, CoreTags coreTags = CoreTags.Component) : ComponentAttribute(assetId, instanceType, coreTags, typeof(T1), typeof(T2), typeof(T3));
