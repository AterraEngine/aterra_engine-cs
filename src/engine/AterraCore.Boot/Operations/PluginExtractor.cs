﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Attributes.AssetVault;
using AterraCore.Common.Attributes.DI;
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
            components.ServiceDescriptors.AddLastRepeated(
                plugin.GetOfAttribute<InjectableAttribute>()
                    .SelectMany(tuple => tuple.Attribute.Interfaces.Select(@interface => (tuple.Type, tuple.Attribute, Interface: @interface)))
                    .Select(tuple => new ServiceDescriptor(
                        tuple.Interface,
                        tuple.Type,
                        tuple.Attribute.Lifetime
                    ))
            );
            #endregion
            #region Import Nexities Asset Factories from Assembly
            components.ServiceDescriptors.AddLastRepeated(
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
