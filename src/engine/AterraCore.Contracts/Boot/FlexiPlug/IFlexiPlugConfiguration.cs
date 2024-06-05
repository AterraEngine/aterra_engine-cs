// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.ConfigFiles.EngineConfig;
using AterraCore.Common.Data;
using AterraCore.Contracts.FlexiPlug;

namespace AterraCore.Contracts.Boot.FlexiPlug;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public interface IFlexiPlugConfiguration : IBootConfiguration {
    public IPluginLoader PluginLoader { get; }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public IFlexiPlugConfiguration CheckAndIncludeRootAssembly();
    public IFlexiPlugConfiguration PreLoadPlugins();
}
