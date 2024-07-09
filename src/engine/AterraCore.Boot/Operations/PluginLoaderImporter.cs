// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.ConfigFiles.PluginConfig;
using AterraCore.Common.Data;
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Nexities.Data.Assets;
using AterraCore.DI;
using AterraCore.FlexiPlug.Attributes;
using AterraCore.Loggers;
using CodeOfChaos.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Xml.Elements;
using static AterraCore.Common.Data.PredefinedAssetIds.NewBootOperationNames;
using static AterraCore.Common.Data.PredefinedAssetIds.NewConfigurationWarnings;

namespace AterraCore.Boot.Operations;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class PluginLoaderImporter : IBootOperation {
    public AssetId AssetId => PluginLoaderImporterOperation;
    public AssetId? RanAfter => PluginLoaderPreChecksOperation;
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

        components.PluginLoader
            #region Import Dynamic injectables from Assembly
            .IterateOverValid((_, plugin) => {
                components.DynamicServices.AddLastRepeated(
                plugin.GetOfAttribute<InjectableAttribute>()
                    .Where(tuple => !tuple.Attribute.IsStatic)
                    .Select(tuple => new ServiceDescriptor(
                        tuple.Attribute.Interface,
                        tuple.Type,
                        tuple.Attribute.Lifetime
                    ))
                );
            })
            #endregion
            #region Import Static injectables from Assembly
            .IterateOverValid((_, plugin) => {
                components.StaticServices.AddLastRepeated(
                plugin.GetOfAttribute<InjectableAttribute>()
                    .Where(tuple => tuple.Attribute.IsStatic)
                    .Select(tuple => new ServiceDescriptor(
                        tuple.Attribute.Interface,
                        tuple.Type,
                        tuple.Attribute.Lifetime
                    ))
                );
            })
            #endregion
            #region Import Nexities Asset Factories from Assembly
            .IterateOverValid((_, plugin) => {
                components.DynamicServices.AddLastRepeated(
                plugin.GetOfAttribute<IAssetAttribute>()
                    .SelectMany(tuple => tuple.Attribute.InterfaceTypes.Select(i => (tuple.Type, tuple.Attribute, Interface: i))
                        .Select(valueTuple => new ServiceDescriptor(
                            valueTuple.Interface,
                            factory: _ => EngineServices.CreateNexitiesAsset<IAssetInstance>(valueTuple.Type) , 
                            valueTuple.Attribute.Lifetime
                        ))
                    )
                );
            })
            #endregion
        ;
    }

}
