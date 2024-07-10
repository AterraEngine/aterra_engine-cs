// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.ConfigFiles.PluginConfig;
using AterraCore.Common.Data;
using AterraCore.Contracts.Nexities.Data.Assets;
using AterraCore.DI;
using AterraCore.FlexiPlug.Attributes;
using AterraCore.Loggers;
using CodeOfChaos.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Xml.Elements;
using static AterraCore.Common.Data.PredefinedAssetIds.NewConfigurationWarnings;

namespace AterraCore.Boot.Operations;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class PluginLoaderZipImporter : IBootOperation {
    private ILogger Logger { get; } = StartupLogger.CreateLogger(false).ForPluginLoaderContext("Importer");

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void Run(IBootOperationComponents components) {
        Logger.Debug("Entered Plugin Loader Importer with {i} valid plugins", components.PluginLoader.GetValidPlugins().Count());

        components.PluginLoader.IterateOverValidWithZipImporter(
            #region Get Files & Config
            (_, plugin, zipImporter) => {
                plugin.InternalFilePaths = zipImporter.GetFileNamesInZip();
                if (!zipImporter.TryGetPluginConfig(out PluginConfigXml? pluginConfig)) {
                    plugin.SetInvalid();
                    components.WarningAtlas.RaiseEvent(NoPluginConfigXmlFound);
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
                        components.WarningAtlas.RaiseEvent(AssemblyCouldNotBeLoaded);
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
