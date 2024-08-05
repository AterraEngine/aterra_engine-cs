// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using AterraCore.OmniVault.Assets.Attributes;
using JetBrains.Annotations;

namespace AterraCore.Nexities.Systems;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class SystemAttribute(
    string assetId,
    CoreTags coreTags = CoreTags.System,
    params Type[] interfaceTypes
) : AssetAttribute(
    assetId,
    coreTags | CoreTags.System,
    interfaceTypes
);

[UsedImplicitly]
public class SystemAttribute<TInterface>( string assetId, CoreTags coreTags = CoreTags.Entity ) : SystemAttribute(assetId, coreTags, typeof(TInterface));
