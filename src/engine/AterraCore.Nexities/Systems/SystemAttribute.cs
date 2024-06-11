// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Common.Types.Nexities;
using AterraCore.Nexities.Assets;

namespace AterraCore.Nexities.Systems;

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
