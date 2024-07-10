// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.ConfigFiles.PluginConfig;
using AterraCore.Common.Types.FlexiPlug;
using System.Reflection;

namespace AterraCore.Contracts.Boot.FlexiPlug;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IAssemblyLoadedPluginDto : IPluginDto {
    Assembly Assembly { get;}
    IEnumerable<Type> Types { get; }
}
