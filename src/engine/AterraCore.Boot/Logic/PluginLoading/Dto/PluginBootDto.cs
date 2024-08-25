// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.ConfigFiles.PluginConfig;
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Boot.Logic.PluginLoading.Dto;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace AterraCore.Boot.Logic.PluginLoading.Dto;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class PluginBootDto : IPluginBootDto {
    private PluginConfigXml? _configXml;

    private readonly List<Assembly> _assemblies = [];
    public IReadOnlyCollection<Assembly> Assemblies => _assemblies.AsReadOnly();

    private readonly List<Type> _types = [];
    public IReadOnlyCollection<Type> Types => _types.AsReadOnly();

    public PluginId PluginNameSpaceId { get; internal set; }

    public bool IsValid { get; private set; } = true;

    public string FilePath { get; init; } = string.Empty;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void SetInvalid() {
        IsValid = false;
    }

    public bool TryGetPluginConfig([NotNullWhen(true)] out PluginConfigXml? pluginConfigXml) {
        pluginConfigXml = _configXml;
        return _configXml is not null;
    }

    public bool TrySetPluginConfig(PluginConfigXml pluginConfigXml) {
        if (_configXml is not null) return false;
        _configXml = pluginConfigXml;
        PluginNameSpaceId = pluginConfigXml.NameSpace;
        return true;
    }

    public void UpdateAssemblies(IEnumerable<Assembly> assemblies) {
        _assemblies.Clear();
        _assemblies.AddRange(assemblies);
        _types.Clear();
        _types.AddRange(Assemblies.SelectMany(assembly => assembly.GetTypes()).AsParallel());
    }

    public IEnumerable<(Type Type, T Attribute)> GetOfAttribute<T>() where T : Attribute {
        return _types
            .SelectMany(type => type
                .GetCustomAttributes<T>(false)// this way we only get the attribute once
                .Select(attribute => (Type: type, Attribute: attribute))
            );
    }
}
