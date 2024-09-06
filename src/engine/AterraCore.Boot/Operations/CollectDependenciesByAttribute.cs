// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Attributes;
using AterraCore.ConfigMancer;
using AterraCore.Contracts.Boot.Operations;
using AterraCore.Loggers;
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
            typeof(XmlPools).Assembly,
            typeof(ConfigAtlas).Assembly,
            typeof(Engine).Assembly
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
