// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Boot.Logic.PluginLoading;
using AterraCore.Contracts.Boot.Operations;
using AterraCore.Contracts.OmniVault.Assets;
using AterraCore.DI;
using AterraCore.FlexiPlug.Attributes;
using AterraCore.Loggers;
using CodeOfChaos.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace AterraCore.Boot.Operations;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class PluginExtractor : IBootOperation {
    private ILogger Logger { get; } = StartupLogger.CreateLogger(false).ForBootOperationContext(typeof(PluginExtractor));

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void Run(IBootComponents components) {
        foreach (IPluginDto plugin in components.ValidPlugins) {
            #region Import Dynamic injectables from Assembly
            components.DynamicServices.AddLastRepeated(
            plugin.GetOfAttribute<InjectableAttribute>()
                .Where(tuple => !tuple.Attribute.IsStatic)
                .Select(tuple => new ServiceDescriptor(
                    tuple.Attribute.Interface,
                    tuple.Type,
                    tuple.Attribute.Lifetime
                ))
            );
            #endregion
            #region Import Static injectables from Assembly
            components.StaticServices.AddLastRepeated(
            plugin.GetOfAttribute<InjectableAttribute>()
                .Where(tuple => tuple.Attribute.IsStatic)
                .Select(tuple => new ServiceDescriptor(
                    tuple.Attribute.Interface,
                    tuple.Type,
                    tuple.Attribute.Lifetime
                ))
            );
            #endregion
            #region Import Nexities Asset Factories from Assembly
            components.DynamicServices.AddLastRepeated(
            plugin.GetOfAttribute<IAssetAttribute>()
                .SelectMany(tuple => tuple.Attribute.InterfaceTypes.Select(i => (tuple.Type, tuple.Attribute, Interface: i))
                    .Select(valueTuple => new ServiceDescriptor(
                        valueTuple.Interface,
                        factory: provider => provider.GetRequiredService<IAssetInstanceAtlas>()
                            .TryCreate(valueTuple.Type, out IAssetInstance? instance)
                            ? instance
                            : throw new InvalidOperationException("Object could not be created"),  
                        valueTuple.Attribute.Lifetime
                    ))
                )
            );
            #endregion
        }
    }
}
