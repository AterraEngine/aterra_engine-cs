// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

namespace AterraCore.Nexities.Components;

using Assets;
using AterraCore.Common.Nexities;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class ComponentAttribute(
    string partialId,
    ServiceLifetimeType instanceType = ServiceLifetimeType.Multiple,
    CoreTags coreTags = CoreTags.Component,
    Type? @interface = null) : AssetAttribute(
    partialId,
    instanceType,
    coreTags | CoreTags.Component,
    interfaceType:@interface
);

public class ComponentAttribute<TInterface>(
    string partialId,
    ServiceLifetimeType instanceType = ServiceLifetimeType.Multiple,
    CoreTags coreTags = CoreTags.Component
) : ComponentAttribute(partialId,
    instanceType,
    coreTags, 
    @interface:typeof(TInterface)
    );