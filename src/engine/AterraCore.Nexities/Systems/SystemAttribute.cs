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
    string assetId,
    ServiceLifetimeType instanceType = ServiceLifetimeType.Singleton,
    CoreTags coreTags = CoreTags.System
    ) : AssetAttribute(
assetId,
instanceType,
coreTags | CoreTags.System
);
