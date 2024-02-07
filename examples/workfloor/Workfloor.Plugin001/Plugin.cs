// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using OldAterraEngine.Contracts.Plugin;

using EnginePlugin_Test.Data;
namespace EnginePlugin_Test;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class Plugin: AEnginePlugin {
    public override string NameReadable => "Test Plugin for WORKFLOOR";
    
    public override Type PluginTextures => typeof(PluginTextures);
    public override Type PluginAssets => typeof(PluginAssets);
}