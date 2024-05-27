// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

namespace AterraCore.Nexities.Entities;

using Assets;
using Common.Types.Nexities;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[AttributeUsage(AttributeTargets.Class)]
public class PooledEntityAttribute(
    string partialId,
    CoreTags coreTags = CoreTags.Entity
) : AssetAttribute(
    partialId,
    ServiceLifetimeType.Pooled,
    coreTags | CoreTags.Entity
);