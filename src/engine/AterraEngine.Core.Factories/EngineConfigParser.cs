// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using AterraEngine.Contracts.DTOs.EngineConfig;
using AterraEngine.Contracts.EngineFactory.Config;
using AterraEngine.Lib.Paths;

namespace AterraEngine.Core.Factories;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <inheritdoc/>
public class EngineConfigParser<T>:IEngineConfigParser<T> where T : EngineConfigDto {
    private readonly XmlSerializer _serializer = new(typeof(T), "urn:aterra-engine:engine-config");
    private readonly XmlReaderSettings _readerSettings = new();
    private readonly XmlWriterSettings _writerSettings = new();
    
    // -----------------------------------------------------------------------------------------------------------------
    // Constructor
    // -----------------------------------------------------------------------------------------------------------------
    public EngineConfigParser() {
        // Reader Settings
        _readerSettings.Schemas.Add("urn:attera-engine:engine-config", PathsStatic.XsdEngineConfig);
        _readerSettings.ValidationType = ValidationType.Schema;
        
        // Write Settings
        _writerSettings.Indent = true;
        _writerSettings.Encoding = Encoding.UTF32;
        _writerSettings.OmitXmlDeclaration = false;
    }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public bool TryDeserializeFromFile(string filePath, out T? engineConfig) {
        // Default to null
        engineConfig = default;
        try {
            if (!File.Exists(filePath)) {
                return false;
            }
            
            using StreamReader reader = new StreamReader(filePath);
            using XmlReader xmlReader = XmlReader.Create(reader, _readerSettings);
            engineConfig = (T)_serializer.Deserialize(xmlReader)!;
            
            return true;

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
            using XmlWriter xmlWriter = XmlWriter.Create(writer, _writerSettings);
            _serializer.Serialize(xmlWriter, engineConfig);
            return true;
        }
        catch (Exception e) {
            Console.WriteLine($"An unexpected error occurred: {e.Message}");
            return false;
        }
        
    }
}

