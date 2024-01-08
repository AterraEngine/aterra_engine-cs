// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Xml.Serialization;

using AterraEngine.Interfaces;
namespace AterraEngine.Config;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class EngineConfigParser<T>:IEngineConfigParser where T : EngineConfig {
    private readonly XmlSerializer _serializer = new(typeof(T));
    
    
    public bool TryDeserializeFromFile(string filePath, out T? engineConfig) {
        // Default to null
        engineConfig = null;
        try {
            if (!File.Exists(filePath)) {
                return false;
            }
            
            using StreamReader reader = new StreamReader(filePath);
            engineConfig = (T)_serializer.Deserialize(reader)!;
            return true;

        }
        catch (InvalidOperationException e) {
            return false;
        }
        catch (Exception e)
        {
            // Handle other exceptions
            Console.WriteLine($"An unexpected error occurred: {e.Message}");
            return false;
        }
    }

    public bool TrySerializeToFile(T engineConfig, string filePath) {
        // Default to null
        try {
            using StreamWriter writer = new StreamWriter(filePath);
            _serializer.Serialize(writer, engineConfig);
            return true;
        }
        catch (Exception e) {
            Console.WriteLine(e);
            return false;
        }
        
    }
}

