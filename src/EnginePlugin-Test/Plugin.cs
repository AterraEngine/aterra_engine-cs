// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.OldContracts.Plugin;

using EnginePlugin_Test.Data;
namespace EnginePlugin_Test;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class Plugin: AEnginePlugin {
    public override string NameReadable => "Test Plugin";
    
    public override Type PluginTextures => typeof(PluginTextures);
    public override Type PluginAssets => typeof(PluginAssets);
}