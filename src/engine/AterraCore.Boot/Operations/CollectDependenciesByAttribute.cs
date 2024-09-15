// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Attributes;
using AterraCore.ConfigMancer;
using AterraCore.Contracts.Boot.Operations;
using AterraCore.FlexiPlug;
using AterraCore.Loggers;
using AterraCore.OmniVault.Assets;
using AterraCore.OmniVault.DataCollector;
using AterraCore.OmniVault.Textures;
using AterraCore.OmniVault.World;
using AterraCore.PoolCorps;
using AterraEngine;
using CodeOfChaos.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace AterraCore.Boot.Operations;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class CollectDependenciesByAttribute : IBootOperation {
    private ILogger Logger { get; } = StartupLogger.CreateLogger(false).ForBootOperationContext(nameof(CollectDependenciesByAttribute));

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void Run(IBootComponents components) {
        Logger.Debug("Entered Collection of Dependencies");

        List<Assembly> assemblies = [
            typeof(AssetAtlas).Assembly,
            typeof(AterraCoreWorld).Assembly,
            typeof(ConfigAtlas).Assembly,
            typeof(DataCollector).Assembly,
            typeof(Engine).Assembly,
            typeof(PluginAtlas).Assembly,
            typeof(TextureAtlas).Assembly,
            typeof(XmlPools).Assembly
        ];

        IEnumerable<ServiceDescriptor> dependencies = assemblies
            .SelectMany(assembly => assembly
                .GetTypes()
                .Select(type => (type, Attributes: type.GetCustomAttributes<InjectableAttribute>()))
                .Where(tuple => tuple.type is { IsClass: true, IsAbstract: false } && tuple.Attributes.Any())
                .SelectMany(tuple => tuple.Attributes
                    .SelectMany(attribute => attribute.Interfaces.Select(@interface => (tuple.type, attribute, Interface: @interface)))
                )
                .Select(tuple => new ServiceDescriptor(
                    tuple.Interface,
                    tuple.type,
                    tuple.attribute.Lifetime
                ))
            );

        components.ServiceDescriptors.AddLastRepeated(dependencies);

    }
}
