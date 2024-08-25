// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Attributes;
using AterraCore.Contracts.Boot.Logic.PluginLoading.Dto;
using AterraCore.Contracts.Boot.Operations;
using AterraCore.Contracts.OmniVault.Assets;
using CodeOfChaos.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace AterraCore.Boot.Operations;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class PluginExtractor : IBootOperation {
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void Run(IBootComponents components) {
        foreach (IPluginBootDto plugin in components.ValidPlugins) {
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
                plugin.GetOfAttribute<AssetAttribute>()
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
