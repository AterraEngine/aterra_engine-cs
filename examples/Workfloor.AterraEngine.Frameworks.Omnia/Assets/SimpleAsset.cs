// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Frameworks.Omnia;

namespace Workfloor.AterraEngine.Frameworks.Omnia.Assets;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class SimpleAsset : OmniaAsset {
    public string Name { get; set; } = string.Empty;

    public override void Initialize(OmniaId assetId) {
        base.Initialize(assetId);
        
        Name = "Something Simple";
    }

    public override bool Cleanup() {
        base.Cleanup();
        
        Name = string.Empty;
        return true;
    }
}
