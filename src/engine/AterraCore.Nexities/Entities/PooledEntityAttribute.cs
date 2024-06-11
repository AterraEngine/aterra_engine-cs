// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Common.Types.Nexities;
using AterraCore.Nexities.Assets;

namespace AterraCore.Nexities.Entities;

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
