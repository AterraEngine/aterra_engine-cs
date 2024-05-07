﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Nexities.Assets;
using AterraCore.Common.Nexities;

namespace AterraCore.Nexities.Components;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class ComponentAttribute(
    string partialId,
    AssetInstanceType instanceType = AssetInstanceType.Multiple,
    CoreTags coreTags = CoreTags.Component,
    Type? @interface = null) : AssetAttribute(
    partialId,
    instanceType,
    coreTags | CoreTags.Component
) {
    public Type? Interface { get; } = @interface;
}

public class ComponentAttribute<TInterface>(
    string partialId,
    AssetInstanceType instanceType = AssetInstanceType.Multiple,
    CoreTags coreTags = CoreTags.Component
) : ComponentAttribute(partialId,
    instanceType,
    coreTags, typeof(TInterface));