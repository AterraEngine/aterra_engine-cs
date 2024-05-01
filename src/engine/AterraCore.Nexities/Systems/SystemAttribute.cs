// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Common.Nexities;
using AterraCore.Nexities.Assets;

namespace AterraCore.Nexities.Systems;

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