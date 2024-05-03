// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Common.Nexities;
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
    AssetInstanceType.Pooled, 
    coreTags | CoreTags.Entity
);