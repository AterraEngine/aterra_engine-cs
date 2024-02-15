﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Contracts.Core.PluginFramework;
using AterraEngine.Core.Types;

namespace AterraEngine.Lib.Plugin;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

// Whenever a plugin class is constructed, the EngineServices.Provider hasn't been built yet
//  Meaning that DI doesn't work here
public abstract class Plugin(PluginId id) : IPlugin {
    public PluginId Id { get; } = id;

    public virtual IPluginServiceBuilder?  Services    => null;
    public virtual IPluginAssetsBuilder?    Assets      => null;
    public virtual IPluginTexturesBuilder?  Textures    => null;
}