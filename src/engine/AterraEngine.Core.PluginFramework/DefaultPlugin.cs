// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Types;
using AterraEngine.Contracts.Core.PluginFramework;
using AterraEngine.Core.ServicesFramework;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace AterraEngine.Core.PluginFramework;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class DefaultPlugin<TAssets>(PluginId id, string name) : IPlugin where TAssets : DefaultPluginAssets {
    public PluginId Id { get; } = id;
    public string Name { get; private set; } = name;
    
    private bool IsAssigned { get; set; } // default is false
    private PluginDto PluginDto { get; } = new(id, name);
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public virtual void AssignServices(IServiceCollection serviceCollection) { }
    
    public void AssignAssets(ILogger startupLogger) {
        if (IsAssigned) {
            const string error = "Assets have already been assigned."; 
            startupLogger.Fatal(error);
            throw new InvalidOperationException(error);
        }
        
        TAssets assets = EngineServices.CreateWithServices<TAssets>();
        assets.ParentPluginDto = PluginDto; // needed for populate of AssetIds, etc...
        assets.AssignAssets();
        IsAssigned = true;
    }
}