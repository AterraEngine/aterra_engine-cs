// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Nexities.Assets;
using AterraCore.Common.Nexities;

namespace AterraCore.Nexities.Entities;

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