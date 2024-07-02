// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Common.Types.Nexities;
using AterraCore.Nexities.Assets;

namespace AterraCore.Nexities.Components;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class ComponentAttribute(
    string assetId,
    ServiceLifetimeType instanceType = ServiceLifetimeType.Multiple,
    CoreTags coreTags = CoreTags.Component,
    Type? @interface = null) : AssetAttribute(
assetId,
instanceType,
coreTags | CoreTags.Component,
@interface
);

public class ComponentAttribute<TInterface>(
    string assetId,
    ServiceLifetimeType instanceType = ServiceLifetimeType.Multiple,
    CoreTags coreTags = CoreTags.Component
    ) : ComponentAttribute(assetId,
instanceType,
coreTags,
typeof(TInterface)
);
