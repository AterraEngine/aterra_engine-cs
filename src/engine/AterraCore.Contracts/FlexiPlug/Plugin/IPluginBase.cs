// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Reflection;
using AterraCore.Common.FlexiPlug;
namespace AterraCore.Contracts.FlexiPlug.Plugin;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public interface IPluginBase {
    public PluginId Id { get; }
    public string ReadableName { get; }
    public List<Assembly> Assemblies { get; }

    public IEnumerable<Type> Types { get; }
}