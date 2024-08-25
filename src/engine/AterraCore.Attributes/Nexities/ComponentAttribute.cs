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
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
public class ComponentAttribute(
    string assetId,
    CoreTags coreTags = CoreTags.Component,
    ServiceLifetime lifetime = ServiceLifetime.Transient,
    params Type[] @interface)
: AssetAttribute(
    assetId,
    coreTags | CoreTags.Component,
    lifetime,
    @interface
);

[UsedImplicitly] public class ComponentAttribute<TInterface>(string assetId, CoreTags coreTags = CoreTags.Component, ServiceLifetime lifetime = ServiceLifetime.Transient) : ComponentAttribute(assetId, coreTags,lifetime, typeof(TInterface));
[UsedImplicitly] public class ComponentAttribute<T1, T2>(string assetId, CoreTags coreTags = CoreTags.Component, ServiceLifetime lifetime = ServiceLifetime.Transient) : ComponentAttribute(assetId, coreTags,lifetime, typeof(T1), typeof(T2));
[UsedImplicitly] public class ComponentAttribute<T1, T2, T3>(string assetId, CoreTags coreTags = CoreTags.Component, ServiceLifetime lifetime = ServiceLifetime.Transient) : ComponentAttribute(assetId, coreTags,lifetime, typeof(T1), typeof(T2), typeof(T3));
[UsedImplicitly] public class ComponentAttribute<T1, T2, T3, T4>(string assetId, CoreTags coreTags = CoreTags.Component, ServiceLifetime lifetime = ServiceLifetime.Transient) : ComponentAttribute(assetId, coreTags,lifetime, typeof(T1), typeof(T2), typeof(T3), typeof(T4));
[UsedImplicitly] public class ComponentAttribute<T1, T2, T3, T4, T5>(string assetId, CoreTags coreTags = CoreTags.Component, ServiceLifetime lifetime = ServiceLifetime.Transient) : ComponentAttribute(assetId, coreTags,lifetime, typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5));

