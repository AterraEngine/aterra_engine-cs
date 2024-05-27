// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Reflection;

namespace AterraCore.Contracts.FlexiPlug.Plugin;

using Common.Types.FlexiPlug;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public interface IPluginBase {
    public PluginId Id { get; }
    public string ReadableName { get; }
    public List<Assembly> Assemblies { get; }

    public IEnumerable<Type> Types { get; }
}