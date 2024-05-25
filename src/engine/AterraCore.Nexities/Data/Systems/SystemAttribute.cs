// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

namespace AterraCore.Nexities.Data.Systems;

using Assets;
using AterraCore.Common.Nexities;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class SystemAttribute(
    string partialId,
    AssetInstanceType instanceType = AssetInstanceType.Singleton,
    CoreTags coreTags = CoreTags.System
) : AssetAttribute(
    partialId,
    instanceType,
    coreTags | CoreTags.System
);