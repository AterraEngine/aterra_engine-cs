// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Reflection;
using AterraCore.Contracts.FlexiPlug;
using AterraCore.Contracts.FlexiPlug.Plugin;
using AterraCore.FlexiPlug.Plugin;
using Serilog;

namespace AterraCore.FlexiPlug;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public class FlexiPlugLoader(ILogger logger) {
    private LinkedList<IPluginData> _plugins = [];
    public IReadOnlyCollection<IPluginData> Plugins => _plugins.ToList().AsReadOnly();

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
    public void FindPluginDLLs(string folderPath) {
        try {
            // Grab all DLL files from the folder
            Directory.GetFiles(folderPath, "*.dll")
                .Select(Assembly.LoadFrom)
                .Select(assembly => new PluginData(PluginIdCounter++, assembly)).ToList()
                .ForEach(data => _plugins.AddLast(data));
        }
        
        catch(Exception e) {
            logger.Error(e, "An error occurred while trying to find plugin DLLs"); // todo rewrite so we try and log the error in the LINQ?
            throw;
        }
    }
    
    public void LoadPlugins() {}
}