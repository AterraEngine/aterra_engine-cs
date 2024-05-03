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
    CoreTags coreTags = CoreTags.Entity
) : AssetAttribute(
    partialId,
    CheckNotPooled(instanceType),
    coreTags | CoreTags.Entity
) {
    private static AssetInstanceType CheckNotPooled(AssetInstanceType instanceType) {
        if (instanceType == AssetInstanceType.Pooled) {
            throw new ArgumentException("A Entity cannot be a pooled Asset. Use a 'PooledEntity' instead");
        }

        return instanceType;
    }
}