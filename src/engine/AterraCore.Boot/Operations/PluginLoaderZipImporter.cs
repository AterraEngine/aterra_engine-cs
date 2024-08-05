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
                plugin.InternalFilePaths = zipImporter.GetFileNamesInZip();
                if (!zipImporter.TryGetPluginConfig(out PluginConfigXml? pluginConfig)) {
                    plugin.SetInvalid();
                    Logger.Warning("Plugin config is invalid");
                    return;
                }
                
                plugin.ConfigXml = pluginConfig;
                Logger.Debug("Loaded ConfigXml for {plugin}", plugin.ConfigXml.NameSpace);
            },     
            #endregion
            #region Import Data
            (_, plugin, zipImporter) => {
                // Correct paths
                IEnumerable<FileDto> correctPaths = plugin.ConfigXml.Dlls
                    .Select(binDto => new FileDto{FilePath = Path
                        .Combine(Paths.Plugins.PluginBinFolder, binDto.FilePath)
                        .Replace("\\", "/")
                    }); 
                
                // Extract assembly(s)
                plugin.Assemblies.AddRange(correctPaths
                    .Select(fileDto => {
                        if (zipImporter.TryGetDllAssembly(fileDto, out Assembly? assembly)) return assembly;
                        plugin.SetInvalid();
                        Logger.Warning("Failed to load assembly {file}", fileDto.FilePath);
                        return assembly;
                    })
                    .Where(assembly => assembly != null)
                    .Select(assembly => assembly!)
                );
            }
            #endregion
        );
    }

}
