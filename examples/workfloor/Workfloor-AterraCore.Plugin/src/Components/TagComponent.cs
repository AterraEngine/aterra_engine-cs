// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.AssetVault;
using AterraCore.Common.Attributes.Nexities;
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Nexities.Components;
using JetBrains.Annotations;
using System.Collections.Concurrent;

namespace Workfloor_AterraCore.Plugin.Components;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
[Component<TagComponent>(WorkfloorIdLib.Components.TagComponent)]
public class TagComponent : AssetInstance, INexitiesComponent {
    public ConcurrentBag<AssetTag> Tags { get; set; } = [];
}
