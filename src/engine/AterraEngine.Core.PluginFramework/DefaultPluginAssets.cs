// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Contracts.Core.PluginFramework;
using Serilog;

namespace AterraEngine.Core.PluginFramework;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class DefaultPluginAssets : IPluginAssets {
    public PluginDto ParentPluginDto { get; internal set; } = null!;
    public abstract void AssignAssets();
}
