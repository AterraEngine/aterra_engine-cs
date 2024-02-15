﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Contracts.Core.PluginFramework;
namespace AterraEngine.Lib.Plugin;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class PluginAssetsBuilder : IPluginAssetsBuilder {
    public abstract void LoadAssets();
}