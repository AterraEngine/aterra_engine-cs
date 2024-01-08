// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Ansi;
using AterraEngine.Config;
using AterraEngine.Plugin;

namespace AterraEngine_Workfloor;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ConfigTest {
    const string _engineConfigXml = "resources/engine_config-example.xml";
    const string _engineConfigXmlOutput = "resources/engine_config-output.xml";

    public void Main() {
        // EngineConfig Test
        EngineConfigParser<EngineConfig> engineConfigParser = new EngineConfigParser<EngineConfig>();
        
        if (!engineConfigParser.TryDeserializeFromFile(_engineConfigXml, out EngineConfig? engineConfig)
            || engineConfig is null) {
            throw new Exception("File coule not be parsed");
        }
        
        // Plugin Test
        EnginePluginManager enginePluginManager = new EnginePluginManager();

        if (!enginePluginManager.TryLoadOrderFromEngineConfig(engineConfig: engineConfig, out _)) {
            throw new Exception("SOMETHING WENT WRONG!!!");
        }
        enginePluginManager.LoadPlugins();
    }

}