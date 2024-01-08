// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Config;

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

        
        // Input XML config
        using StreamReader reader1 = new StreamReader(_engineConfigXml);
        Console.WriteLine(reader1.ReadToEnd());
        
        if (!engineConfigParser.TryDeserializeFromFile(_engineConfigXml, out EngineConfig? engineConfig)
            || engineConfig is null) {
            throw new Exception("File coule not be parsed");
        }
        
        // Output XML config
        // sshh this is not a good idea in the long run 
        using StreamReader reader2 = new StreamReader(_engineConfigXmlOutput);
        Console.WriteLine(reader2.ReadToEnd());
    }

}