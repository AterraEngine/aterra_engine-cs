// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using AterraCore.OmniVault.Assets.Attributes;
using JetBrains.Annotations;

namespace AterraCore.Nexities.Components;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
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
[UsedImplicitly] public class ComponentAttribute<T1, T2, T3, T4>(string assetId, CoreTags coreTags = CoreTags.Component) : ComponentAttribute(assetId, coreTags, typeof(T1), typeof(T2), typeof(T3), typeof(T4));

