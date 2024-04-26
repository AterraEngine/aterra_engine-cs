// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Text;
using System.Xml;
using System.Xml.Serialization;
using AterraCore.Contracts.Config;

namespace AterraCore.Config;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public class XsdGenerator : IXsdGenerator {
    public void GenerateXsd<T>(string nameSpace, bool prettify, string outputPath) => GenerateXsd(typeof(T), nameSpace, prettify, outputPath);
    public void GenerateXsd(Type type, string nameSpace, bool prettify, string outputPath) {
        var importer = new XmlReflectionImporter(null, nameSpace);
        var schemas = new XmlSchemas();
        var exporter = new XmlSchemaExporter(schemas);
        
        XmlTypeMapping map = importer.ImportTypeMapping(type);
        exporter.ExportTypeMapping(map);
        
        using var writer = XmlWriter.Create(
            outputPath, 
            new XmlWriterSettings {
                Indent = prettify,
                Encoding = Encoding.UTF32,
            }
        );
        
        schemas[0].Write(writer);
    }
}