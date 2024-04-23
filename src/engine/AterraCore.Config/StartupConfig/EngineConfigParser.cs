// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using AterraCore.Common;
using AterraCore.Contracts.StartupConfig;
using Serilog;

namespace AterraCore.Config.StartupConfig;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <inheritdoc/>
public class EngineConfigParser<T>(ILogger logger):IEngineConfigParser<T> where T : EngineConfigDto {
    private readonly XmlSerializer _serializer = new(typeof(T), "urn:aterra-engine:engine-config");
    private readonly XmlReaderSettings _readerSettings = DefineReaderSettings(logger);
    private readonly XmlWriterSettings _writerSettings = new() {
        Indent = true,
        Encoding = Encoding.UTF32,
        OmitXmlDeclaration = false,
    };
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    private static XmlReaderSettings DefineReaderSettings(ILogger logger) {
        var schemas = new XmlSchemaSet();
        schemas.Add("urn:aterra-engine:engine-config", XmlReader.Create(Paths.XsdEngineConfigDto));

        var settings = new XmlReaderSettings {
            ValidationType = ValidationType.Schema,
            ValidationFlags = XmlSchemaValidationFlags.ProcessInlineSchema 
                              | XmlSchemaValidationFlags.ProcessSchemaLocation
                              | XmlSchemaValidationFlags.ReportValidationWarnings,
            Schemas = schemas
        };

        settings.ValidationEventHandler += (sender, args) => {
            switch (args) {
                case { Severity: XmlSeverityType.Warning }:
                    logger.Warning(args.Message);
                    break;
                case { Severity: XmlSeverityType.Error }:
                    logger.Error(args.Message);
                    break;
            }
        };

        return settings;
    }
    
    public bool TryDeserializeFromFile(string filePath, out T? engineConfig) {
        // Default to null
        engineConfig = default;
        try {
            if (!File.Exists(filePath)) {
                logger.Warning("No file found at {FilePath}", filePath);
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

