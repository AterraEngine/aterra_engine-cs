// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using Serilog;
using System.Text;
using System.Xml;
using Xml.Contracts;

namespace Xml;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class XsdGenerator<T>(ILogger logger) : IXsdGenerator {
    private readonly Type _type = typeof(T);

    public void GenerateXsd(string nameSpace, bool prettify, string outputPath) {

        logger.Information("Generating XML Schema (XSD) for object type: {Name}.", _type.FullName);

        var importer = new XmlReflectionImporter(null, nameSpace);
        var schemas = new XmlSchemas();
        var exporter = new XmlSchemaExporter(schemas);

        try {
            logger.Debug("Importing type mapping...");
            XmlTypeMapping map = importer.ImportTypeMapping(_type);

            logger.Debug("Exporting type mapping...");
            exporter.ExportTypeMapping(map);

            logger.Debug("Writing XML...");
            using var writer = XmlWriter.Create(
            outputPath,
            new XmlWriterSettings {
                Indent = prettify,
                Encoding = Encoding.UTF32
            }
            );

            schemas[0].Write(writer);
            logger.Information("XML Schema (XSD) generated successfully at {Path}.", outputPath);
        }
        catch (Exception ex) {
            logger.Error(ex, "An error occurred while generating XML Schema (XSD): {Message}", ex.Message);
        }


    }
}
