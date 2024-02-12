// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using AterraEngine.Contracts.Core.Startup.Config;
using AterraEngine.Core.Startup.EngineConfig.Dto;
using Serilog;

namespace AterraEngine.Core.Startup.EngineConfig;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <inheritdoc/>
public class EngineConfigParser<T>(ILogger logger):IEngineConfigParser<T> where T : EngineConfigDto {
    private readonly XmlSerializer _serializer = new(typeof(T), "urn:aterra-engine:engine-config");
    private readonly XmlReaderSettings _readerSettings = new();
    private readonly XmlWriterSettings _writerSettings = new() {
        Indent = true,
        Encoding = Encoding.UTF32,
        OmitXmlDeclaration = false,
    };
    
    // -----------------------------------------------------------------------------------------------------------------
    // Constructor
    // -----------------------------------------------------------------------------------------------------------------
    // public EngineConfigParser() {
    //     // Reader Settings
    //     // _readerSettings.Schemas.Add("urn:aterra-engine:engine-config", "xsd/engine_config.xsd");
    //     // _readerSettings.ValidationType = ValidationType.Schema;
    //     
    //     // Write Settings
    //     _writerSettings.Indent = true;
    //     _writerSettings.Encoding = Encoding.UTF32;
    //     _writerSettings.OmitXmlDeclaration = false;
    // }
    
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
            
            using var reader = new StreamReader(filePath);
            using var xmlReader = XmlReader.Create(reader, _readerSettings);
            engineConfig = (T)_serializer.Deserialize(xmlReader)!;
            
            return true;

        }
        catch (Exception e)
        {
            // Handle other exceptions
            logger.Error($"An unexpected error occurred: {e.Message}");
            return false;
        }
    }

    public bool TrySerializeToFile(T engineConfig, string filePath) {
        // Default to null
        try {
            using var writer = new StreamWriter(filePath);
            using var xmlWriter = XmlWriter.Create(writer, _writerSettings);
            _serializer.Serialize(xmlWriter, engineConfig);
            return true;
        }
        catch (Exception e) {
            Console.WriteLine($"An unexpected error occurred: {e.Message}");
            return false;
        }
        
    }
}

