// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Common.Attributes.Nexities;
using AterraCore.Nexities.Components;
using JetBrains.Annotations;

namespace Workfloor_AterraCore.Plugin.Components;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Component<BoundingCircle>(WorkfloorIdLib.Components.BoundingCircle)]
[UsedImplicitly]
public class BoundingCircle : NexitiesComponent {
    public float Radius { get; set; } = 1f;
}
