// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Common.Types.Nexities;
using AterraCore.Nexities.Assets;

namespace AterraCore.Nexities.Entities;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class EntityAttribute(
    string partialId,
    ServiceLifetimeType instanceType = ServiceLifetimeType.Multiple,
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
    ServiceLifetimeType instanceType = ServiceLifetimeType.Multiple,
    CoreTags coreTags = CoreTags.Entity
    ) : EntityAttribute(partialId, instanceType, coreTags, typeof(TInterface));
