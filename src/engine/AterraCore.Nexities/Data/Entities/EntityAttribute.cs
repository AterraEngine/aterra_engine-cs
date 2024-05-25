// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

namespace AterraCore.Nexities.Data.Entities;

using Assets;
using AterraCore.Common.Nexities;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class EntityAttribute(
    string partialId,
    AssetInstanceType instanceType = AssetInstanceType.Multiple,
    CoreTags coreTags = CoreTags.Entity,
    Type? @interface = null
) : AssetAttribute(
    partialId,
    instanceType,
    coreTags | CoreTags.Entity
) {
    public Type? Interface { get; } = @interface;
}

public class EntityAttribute<TInterface>(
    string partialId,
    AssetInstanceType instanceType = AssetInstanceType.Multiple,
    CoreTags coreTags = CoreTags.Entity
) : EntityAttribute(partialId, instanceType, coreTags, typeof(TInterface));