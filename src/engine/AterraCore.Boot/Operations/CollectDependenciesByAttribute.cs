// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Boot.Operations;
using AterraCore.Loggers;
using AterraCore.Common.Attributes.DI;
using CodeOfChaos.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace AterraCore.Boot.Operations;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class CollectDependenciesByAttribute : IBootOperation {
    private ILogger Logger { get; } = StartupLogger.CreateLogger(false).ForBootOperationContext(nameof(CollectDependenciesByAttribute));
    private readonly List<Assembly> _assemblies = [];

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    private void AssignFromType<T>() => _assemblies.Add(typeof(T).Assembly);

    public void Run(IBootComponents components) {
        Logger.Debug("Entered Collection of Dependencies");

        AssignFromType<AssetVault.IAssemblyEntry>();
        AssignFromType<ConfigMancer.IAssemblyEntry>();
        AssignFromType<FlexiPlug.IAssemblyEntry>();
        AssignFromType<PoolCorps.IAssemblyEntry>();
        AssignFromType<Threading.IAssemblyEntry>();

        AssignFromType<AterraEngineOLD.IAssemblyEntry>();

        IEnumerable<ServiceDescriptor> dependencies = _assemblies
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
