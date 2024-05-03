// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Common.Nexities;
using AterraCore.Nexities.Entities;
using JetBrains.Annotations;
using Nexities.Lib.Components.Transform2D;

namespace Workfloor_AterraCore.Plugin.Assets;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Entity("1", AssetInstanceType.Pooled)]
[UsedImplicitly]
public class TestEntity(ITransform2DComponent transform2D) : Entity(transform2D), IHasTransform2DComponent {
    public ITransform2DComponent Transform2D { get; } = transform2D;
}