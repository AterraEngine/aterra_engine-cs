// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Data;
using AterraCore.Contracts.Boot.FlexiPlug;
using Extensions;

namespace AterraCore.Boot.FlexiPlug;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public static class FlexiPlugConfigurationMethods {
    public static IFlexiPlugConfiguration ImportPlugins(this IFlexiPlugConfiguration configuration) {
        if (!configuration.PluginLoader.TryParseAllPlugins(configuration.ConfigDto.PluginFilePaths)) {
            configuration.Warnings |= ConfigurationWarnings.PluginLoadOrderUnstable | ConfigurationWarnings.UnstablePlugin;
            configuration.Logger.Warning("Failed to load all plugins correctly.");
        } else {
            configuration.Logger.Information("Plugins successfully loaded.");
        }
        return configuration;
    }

    public static IFlexiPlugConfiguration AssignServices(this IFlexiPlugConfiguration configuration) {
        configuration.PluginLoader.Plugins
            .SelectMany(p => p.GetServices())
            .IterateOver(p => configuration.ServiceDescriptors = configuration.ServiceDescriptors.Append(p));

        configuration.Logger.Information("Assigned Systems from Plugins");
        return configuration;
    }
}
