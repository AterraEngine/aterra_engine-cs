// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Types;
using AterraEngine.Core.PluginFramework;
using Microsoft.Extensions.DependencyInjection;

namespace Workfloor.Data;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class WorkfloorPlugin(PluginId id) : DefaultPlugin<WorkfloorAssets>(id, "Workfloor Plugin") {
    public override void AssignServices(IServiceCollection serviceCollection) {
        
    }
}