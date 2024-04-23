// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.FlexiPlug.Plugin;
using Serilog;

namespace AterraCore.FlexiPlug;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class PluginLoader(ILogger logger) {
    private readonly LinkedList<IPluginData> _plugins = [];
    public LinkedList<IPluginData> Plugins => _plugins;

    private ushort _pluginIdCounter;
    private ushort PluginIdCounter {
        get => _pluginIdCounter;
        set {
            if (value == ushort.MaxValue) {
                var maxId = ushort.MaxValue.ToString("X");
                logger.Fatal("Max Plugin Id of {maxId} is exhausted",maxId);
                throw new OverflowException($"Max Plugin Id of {maxId} is exhausted");
            }
            _pluginIdCounter = value;
        }
    }
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public IEnumerable<string> FindPluginDlls(string folderPath) {
        try {
            // Grab all DLL files from the folder
            return Directory.GetFiles(folderPath, "*.dll", SearchOption.AllDirectories);
            // .Select(Assembly.LoadFrom)
            // .Select(assembly => new PluginData(PluginIdCounter++, assembly)).ToList()
            // .ForEach(data => _plugins.AddLast(data));
        }
        
        catch(Exception e) {
            logger.Error(e, "An error occurred while trying to find plugin DLLs"); // todo rewrite so we try and log the error in the LINQ?
            throw;
        }
    }
}