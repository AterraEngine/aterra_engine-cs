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
    CoreTags coreTags = CoreTags.Component,
    params Type[] @interface)
: AssetAttribute(
    assetId,
    coreTags | CoreTags.Component,
    @interface
);

[UsedImplicitly] public class ComponentAttribute<TInterface>(string assetId, CoreTags coreTags = CoreTags.Component) : ComponentAttribute(assetId, coreTags, typeof(TInterface));
[UsedImplicitly] public class ComponentAttribute<T1, T2>(string assetId, CoreTags coreTags = CoreTags.Component) : ComponentAttribute(assetId, coreTags, typeof(T1), typeof(T2));
[UsedImplicitly] public class ComponentAttribute<T1, T2, T3>(string assetId, CoreTags coreTags = CoreTags.Component) : ComponentAttribute(assetId, coreTags, typeof(T1), typeof(T2), typeof(T3));
