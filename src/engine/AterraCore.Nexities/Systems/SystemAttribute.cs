// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

namespace AterraCore.Nexities.Systems;

using Assets;
using Common.Types.Nexities;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class SystemAttribute(
    string partialId,
    ServiceLifetimeType instanceType = ServiceLifetimeType.Singleton,
    CoreTags coreTags = CoreTags.System
) : AssetAttribute(
    partialId,
    instanceType,
    coreTags | CoreTags.System
);