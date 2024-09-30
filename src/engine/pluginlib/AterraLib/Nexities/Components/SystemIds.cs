// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Attributes.Nexities;

namespace AterraLib.Nexities.Components;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Component<ISystemIds>(StringAssetIdLib.AterraLib.Components.SystemIds)]
[UsedImplicitly]
public class SystemIds : NexitiesComponent, ISystemIds {
    protected virtual List<AssetId> RawAssetIds { get; } = [

    ];

    public IReadOnlyCollection<AssetId> AssetIds => RawAssetIds.AsReadOnly();
}
