// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Nexities.Assets;
using AterraCore.Common;
using AterraCore.Common.Nexities;

namespace AterraCore.Nexities.Entities;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class EntityAttribute(
    string partialId, 
    AssetInstanceType instanceType = AssetInstanceType.Multiple,
    CoreTags coreTags = CoreTags.Entity
) : AssetAttribute(
    partialId, 
    instanceType, 
    coreTags | CoreTags.Entity
);