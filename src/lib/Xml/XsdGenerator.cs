// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using Serilog;
using System.Text;
using System.Xml;
using System.Xml.Schema;

namespace Xml;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class XsdGenerator(ILogger logger) {
    public void GenerateXsd<T>(string nameSpace, bool prettify, string outputPath) => GenerateXsd(typeof(T), nameSpace, prettify, outputPath);
    public void GenerateXsd(Type type, string nameSpace, bool prettify, string outputPath) {

        logger.Information("Generating XML Schema (XSD) for object type: {Name}.", type.FullName);

        var importer = new XmlReflectionImporter(null, nameSpace);
        var schemas = new XmlSchemas();
        var exporter = new XmlSchemaExporter(schemas);
       
        try {
            logger.Debug("Importing type mapping...");
            XmlTypeMapping map = importer.ImportTypeMapping(type);

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

            XmlSchema validSchema = schemas[0];
            validSchema.Write(writer);
            logger.Information("XML Schema (XSD) generated successfully at {Path}.", outputPath);
        }
        catch (Exception ex) {
            logger.Error(ex, "An error occurred while generating XML Schema (XSD): {Message}", ex.Message);
        }


    }
}
