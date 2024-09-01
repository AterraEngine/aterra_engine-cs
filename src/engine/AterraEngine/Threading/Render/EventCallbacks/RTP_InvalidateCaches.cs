// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Nexities.Systems;

namespace AterraEngine.Threading.Render;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public partial class RenderThreadProcessor {
    private void OnEventManagerOnEventClearSystemCaches() {
        if (world.ActiveLevel is not { RenderSystems: var renderSystems }) return;
        foreach (INexitiesSystem system in renderSystems) {
            system.InvalidateCaches();
        }
    }
}
