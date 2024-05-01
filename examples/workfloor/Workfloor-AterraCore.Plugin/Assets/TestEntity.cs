// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Nexities.Entities;
using JetBrains.Annotations;

namespace Workfloor_AterraCore.Plugin.Assets;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Entity("1")]
[UsedImplicitly]
public class TestEntity(ITestComponent testComponent) : Entity(testComponent) , IHasTestComponent {
    public ITestComponent TestComponent { get; } = testComponent;
}