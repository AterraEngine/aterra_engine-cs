// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Reflection;

namespace AterraCore.Common.Types.Nexities;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record struct AssetRegistration(
    AssetId AssetId,
    Type Type
) {
    public IEnumerable<Type> InterfaceTypes { get; init; } = [];

    // Data from IAssetAttribute
    public CoreTags CoreTags { get; init; } = CoreTags.Undefined;

    // Data from IAssetTagAttribute
    public IEnumerable<string> StringTags { get; init; } = [];

    // Data from AbstractOverwritesAssetTagAttribute
    public IEnumerable<AssetId> OverridableAssetIds { get; init; } = [];

    private ConstructorInfo? _constructor = null;
    public ConstructorInfo Constructor => _constructor ??= Type.GetConstructors().First();
    
    private bool? _isSingleton;
    public bool IsSingleton => _isSingleton ??= CoreTags.HasFlag(CoreTags.Singleton);
}
