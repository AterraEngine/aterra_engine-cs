// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

namespace AterraCore.Nexities.Data.Entities;

using Assets;
using AterraCore.Common.Nexities;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[AttributeUsage(AttributeTargets.Class)]
public class PooledEntityAttribute(
    string partialId,
    CoreTags coreTags = CoreTags.Entity
) : AssetAttribute(
    partialId,
    AssetInstanceType.Pooled,
    coreTags | CoreTags.Entity
);