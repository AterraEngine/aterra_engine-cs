﻿// ---------------------------------------------------------------------------------------------------------------------
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
    string partialId,
    ServiceLifetimeType instanceType = ServiceLifetimeType.Multiple,
    CoreTags coreTags = CoreTags.Component,
    Type? @interface = null) : AssetAttribute(
partialId,
instanceType,
coreTags | CoreTags.Component,
@interface
);

public class ComponentAttribute<TInterface>(
    string partialId,
    ServiceLifetimeType instanceType = ServiceLifetimeType.Multiple,
    CoreTags coreTags = CoreTags.Component
    ) : ComponentAttribute(partialId,
instanceType,
coreTags,
typeof(TInterface)
);
