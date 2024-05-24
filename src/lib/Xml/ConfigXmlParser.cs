// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Serilog;
using Xml.Contracts;
namespace Xml;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ConfigXmlParser<T>(ILogger logger, string nameSpace, string xsdPath)
    : IConfigXmlParser<T>
    where T : IConfigDto<T>, new() {
    private readonly XmlReaderSettings _readerSettings = DefineReaderSettings(logger, nameSpace, xsdPath);
    private readonly XmlSerializer _serializer = new(typeof(T), nameSpace);
    private readonly XmlWriterSettings _writerSettings = new() {
        Indent = true,
        Encoding = Encoding.UTF8,
        OmitXmlDeclaration = false
    };

    public bool TryDeserializeFromFile(string filePath, [NotNullWhen(true)] out T? config) {
        // Default to null
        config = default;
        try {
            if (!File.Exists(filePath)) {
                logger.Warning("No file found at {FilePath}", filePath);
                return false;
            }

            using var reader = new StreamReader(filePath);
            using var xmlReader = XmlReader.Create(reader, _readerSettings);
            config = (T)_serializer.Deserialize(xmlReader)!;

            return true;
        }
        catch (Exception e) {
            // Handle other exceptions
            logger.Error($"An unexpected error occurred: {e.Message}");
            return false;
        }
    }

    public bool TrySerializeToFile(T config, string filePath) {
        // Default to null
        try {
            using var writer = new StreamWriter(filePath);
            using var xmlWriter = XmlWriter.Create(writer, _writerSettings);
            _serializer.Serialize(xmlWriter, config);
            return true;
        }
        catch (Exception e) {
            Console.WriteLine($"An unexpected error occurred: {e.Message}");
            return false;
        }

    }

    public bool TrySerializeFromBytes(byte[] bytes, [NotNullWhen(true)] out T? config) {
        config = default;
        try {
            using var memoryStream = new MemoryStream(bytes);
            using var xmlReader = XmlReader.Create(memoryStream, _readerSettings);
            config = (T)_serializer.Deserialize(xmlReader)!;
            return true;
        }

        catch (Exception e) {
            logger.Warning(e, "Memory stream could not parse into a {t}", typeof(T));
            return false;
        }
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    private static XmlReaderSettings DefineReaderSettings(ILogger logger, string nameSpace, string xsdPath) {
        var schemas = new XmlSchemaSet();
        schemas.Add(nameSpace, XmlReader.Create(xsdPath));

        var settings = new XmlReaderSettings {
            ValidationType = ValidationType.Schema,
            ValidationFlags = XmlSchemaValidationFlags.ProcessInlineSchema
                              | XmlSchemaValidationFlags.ProcessSchemaLocation
                              | XmlSchemaValidationFlags.ReportValidationWarnings,
            Schemas = schemas
        };

        settings.ValidationEventHandler += (_, args) => {
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
}