// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.ConfigFiles.PluginConfig;
using AterraCore.Common.Types.FlexiPlug;
using AterraCore.Contracts.Boot.FlexiPlug;
using AterraCore.FlexiPlug.Attributes;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace AterraCore.Boot.FlexiPlug.PluginLoading;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class LoadedPluginDto(int id, string filepath) : ILoadedPluginDto {
    public PluginId Id { get; } = new(id);
    public string FilePath { get; } = filepath;
    public string ReadableName => Data?.ReadableName ?? FilePath;
    
    private string? _readableId;
    public string ReadableId => _readableId ??= $"Plugin.{Id}";
    
    public bool IsProcessed { get; set; }
    public PluginConfigXml? Data { get; set; }

    private PluginValidity _validity = PluginValidity.Untested;
    public PluginValidity Validity {
        get => _validity;
        set => _validity = _validity != PluginValidity.Invalid ? value : _validity;// Once invalid, always invalid
    }

    public string? CheckSum { get; set; } = null;
    
    public List<Assembly> Assemblies { get; } = [];
    private IEnumerable<Type>? _types;
    public IEnumerable<Type> Types => _types ??= Assemblies.SelectMany(assembly => assembly.GetTypes());

    private IEnumerable<(Type Type, InjectableServiceAttribute Attribute)>? _injectableServices;
    private IEnumerable<(Type Type, InjectableServiceAttribute Attribute)> InjectableServices => _injectableServices ??= Types
        .Select(t => (t,  t.GetCustomAttribute<InjectableServiceAttribute>(false)))// this way we only get the attribute once
        .Where(t => t.Item2 != null)!
    ;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public IEnumerable<ServiceDescriptor> GetServicesDefault() => InjectableServices
        .Where(t => !t.Attribute.IsStatic)
        .Select(t => new ServiceDescriptor(
            t.Attribute.Interface,
            t.Type,
            t.Attribute.Lifetime
        ));
    
    public IEnumerable<ServiceDescriptor> GetServicesStatic() => InjectableServices
        .Where(t => t.Attribute.IsStatic)
        .Select(t => new ServiceDescriptor(
            t.Attribute.Interface,
            t.Type,
            t.Attribute.Lifetime
        ));
}
