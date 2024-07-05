// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.ConfigFiles.PluginConfig;
using AterraCore.Common.Types.FlexiPlug;
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Boot.FlexiPlug;
using AterraCore.Contracts.Nexities.Data.Assets;
using AterraCore.DI;
using AterraCore.FlexiPlug.Attributes;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace AterraCore.Boot.FlexiPlug.PluginLoading;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class LoadedPluginDto(ulong id, string filepath) : ILoadedPluginDto {
    public string FilePath { get; } = filepath;
    private ulong TempLoadOrderId { get; } = id;
    public string NameReadable => Data?.NameReadable ?? FilePath;
    public string NameSpace => Data?.NameSpace ?? $"temp_{TempLoadOrderId}";

    public bool IsProcessed { get; set; }
    public PluginConfigXml? Data { get; set; }

    private PluginValidity _validity = PluginValidity.Untested;

    public PluginValidity Validity {
        get => _validity;
        set => _validity = _validity != PluginValidity.Invalid ? value : _validity;// Once invalid, always invalid
    }

    public string? CheckSum { get; set; } = null;
    public IEnumerable<string> InternalFilePaths { get; set; } = [];

    public List<Assembly> Assemblies { get; } = [];
    private IEnumerable<Type>? _types;
    public IEnumerable<Type> Types => _types ??= Assemblies.SelectMany(assembly => assembly.GetTypes());

    private IEnumerable<(Type Type, InjectableAttribute Attribute)>? _injectableServices;
    private IEnumerable<(Type Type, InjectableAttribute Attribute)> InjectableServices => _injectableServices ??= Types
        .SelectMany(type => type
            .GetCustomAttributes<InjectableAttribute>(false)// this way we only get the attribute once
            .Select(attribute => (Type: type, Attribute: attribute))
        );
    
    private IEnumerable<(Type Type, ServiceLifetime Lifetime)>? _nexitiesAssets;
    private IEnumerable<(Type Type, ServiceLifetime Lifetime)> InjectableNexitiesAssetTypes => _nexitiesAssets ??= Types
        .SelectMany(type => type
            .GetCustomAttributes<AbstractAssetAttribute>(false)// this way we only get the attribute once
            .SelectMany(attribute => attribute.InterfaceTypes.Select(t => (Type:t, Lifetime:ServiceLifetime.Transient)))
        );

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public IEnumerable<ServiceDescriptor> GetServicesDefault() {
        return InjectableServices
            .Where(t => !t.Attribute.IsStatic)
            .Select(t => new ServiceDescriptor(
                t.Attribute.Interface,
                factory: _ => EngineServices.CreateNexitiesAsset<IAssetInstance>(t.Attribute.Interface) ,
                t.Attribute.Lifetime
            ))
            
            .Concat(
                InjectableNexitiesAssetTypes
                    .Select(t => new ServiceDescriptor(
                        t.Type,
                        factory: _ => EngineServices.CreateNexitiesAsset<IAssetInstance>(t.Type) ,
                        t.Lifetime
                    )))
        ;
    }

    public IEnumerable<ServiceDescriptor> GetServicesStatic() {
        return InjectableServices
            .Where(t => t.Attribute.IsStatic)
            .Select(t => new ServiceDescriptor(
            t.Attribute.Interface,
            factory: _ => EngineServices.CreateNexitiesAsset<IAssetInstance>(t.Attribute.Interface) ,
            t.Attribute.Lifetime
            ));
    }
}
