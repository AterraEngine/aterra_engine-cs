// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Reflection;
using AterraCore.Contracts.FlexiPlug.Plugin;

namespace AterraCore.Contracts.FlexiPlug;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IPluginLoader {
    public LinkedList<IPluginDto> Plugins { get; }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public bool TryParseAllPlugins(IEnumerable<string> filePaths);
    public void InjectAssemblyAsPlugin(Assembly assembly);

    public LinkedList<IPlugin> ExportToPlugins();
}