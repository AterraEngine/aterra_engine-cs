// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Boot.Logic.PluginLoading;
using System.Reflection;

namespace AterraCore.Boot.Logic.PluginLoading;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class AssemblyLoadedPluginDto(Assembly assembly, PluginId pluginId) : APluginDto, IAssemblyLoadedPluginDto {
    public Assembly Assembly { get; } = assembly;
    
    private IEnumerable<Type>? _types;
    public override IEnumerable<Type> Types => _types ??= Assembly.GetTypes();
    public override PluginId PluginId => pluginId;
}
