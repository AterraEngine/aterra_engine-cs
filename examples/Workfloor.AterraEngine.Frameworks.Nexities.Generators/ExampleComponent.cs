// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Frameworks.Nexities;
using AterraEngine.Frameworks.Omnia;

namespace Workfloor.AterraEngine.Frameworks.Nexities.Generators;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[OmniaId("example:component")]
public class ExampleComponent : NexitiesComponent {
    public int Value { get; set; }
}
