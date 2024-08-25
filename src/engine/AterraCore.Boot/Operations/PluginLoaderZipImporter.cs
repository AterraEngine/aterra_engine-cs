// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.ConfigFiles.PluginConfig;
using AterraCore.Common.Data;
using AterraCore.Contracts.Boot.Operations;
using AterraCore.Loggers;
using System.Reflection;
using Xml.Elements;

namespace AterraCore.Boot.Operations;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class PluginLoaderZipImporter : IBootOperation {
    private ILogger Logger { get; } = StartupLogger.CreateLogger(false).ForPluginLoaderContext("Importer");

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void Run(IBootComponents components) {
        Logger.Debug("Entered Plugin Loader Importer with {i} valid plugins", components.PluginLoader.GetValidPlugins().Count());

        components.PluginLoader.IterateOverValidWithZipImporter(
            #region Get Files & Config
            (_, plugin, zipImporter) => {
                // plugin.InternalFilePaths = zipImporter.GetFileNamesInZip();
                if (!zipImporter.TryGetPluginConfig(out PluginConfigXml? pluginConfig)) {
                    plugin.SetInvalid();
                    Logger.Warning("Plugin config is invalid");
                    return;
                }
                if (!plugin.TrySetPluginConfig(pluginConfig)) {
                    plugin.SetInvalid();
                    Logger.Warning("Plugin config could not be applied to the plugin");
                    return;
                }
                Logger.Debug("Loaded ConfigXml for {plugin}", pluginConfig.NameSpace);
            },
            #endregion
            #region Import Data
            (_, plugin, zipImporter) => {
                // Correct paths
                if (!plugin.TryGetPluginConfig(out PluginConfigXml? xml)) {
                    plugin.SetInvalid();
                    Logger.Warning("Failed to plugin Config");
                    return;
                }

                // Extract assembly(s)
                IEnumerable<Assembly> assemblies = xml.Dlls
                    .Select(binDto => new FileDto {
                        FilePath = Path
                            .Combine(Paths.Plugins.PluginBinFolder, binDto.FilePath)
                            .Replace("\\", "/")
                    })
                    .Select(fileDto => {
                        if (zipImporter.TryGetDllAssembly(fileDto, out Assembly? assembly)) return assembly;
                        plugin.SetInvalid();
                        Logger.Warning("Failed to load assembly {file}", fileDto.FilePath);
                        return assembly;
                    })
                    .Where(assembly => assembly != null)
                    .Select(assembly => assembly!);

                plugin.UpdateAssemblies(assemblies);
            }
            #endregion
        );
    }
}
