// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using AterraCore.Config.PluginConfig;
using AterraCore.Config.StartupConfig;
using CliArgsParser;
using CliArgsParser.Attributes;
using JetBrains.Annotations;

namespace ProductionTools.Commands;

// ---------------------------------------------------------------------------------------------------------------------
// Support Code
// ---------------------------------------------------------------------------------------------------------------------
[SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
public class ArgsOptions : ParameterOptions {
    [ArgValue('c', "classname")] public string ClassName { get; set; } = null!;
    [ArgValue('p', "prettify")] public bool Prettify { get; set; } = true;
    [ArgValue('o', "output")] public string? OutputFile { get; set; }
    [ArgValue('f', "folder")] public string? OutputFolder { get; set; }
}

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class XmlSchemaGenerator : CliCommandAtlas {
    private readonly Dictionary<string, Type> _dictionary = new() {
        {"engine-config", typeof(EngineConfigDto) },
        {"plugin-config", typeof(PluginConfigDto) },
    };
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    private static void GenerateXsd(Type type, ArgsOptions argsOptions) {
        var importer = new XmlReflectionImporter(null, $"urn:aterra-engine:{argsOptions.ClassName.ToLowerInvariant()}");
        var schemas = new XmlSchemas();
        var exporter = new XmlSchemaExporter(schemas);
        
        XmlTypeMapping map = importer.ImportTypeMapping(type);
        exporter.ExportTypeMapping(map);

        var settings = new XmlWriterSettings {
            Indent = argsOptions.Prettify,
            Encoding = Encoding.UTF32,
        };
        
        string outputFileName = Path.Combine(
            argsOptions.OutputFolder ?? string.Empty, 
            argsOptions.OutputFile ?? $"{type.Name}.xsd"
        );
        
        using var writer = XmlWriter.Create(outputFileName, settings);
        schemas[0].Write(writer);
    }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Commands
    // -----------------------------------------------------------------------------------------------------------------
    [CliCommand<ArgsOptions>("generate-xmlschema")]
    [UsedImplicitly]
    public void GenerateXmlSchemaEngineConfig(ArgsOptions argsOptions) {
        if (!_dictionary.TryGetValue(argsOptions.ClassName.ToLowerInvariant(), out Type? type)) {
            Console.WriteLine("No match found for the provided ClassName. Check the ClassName and try again.");
        }
        GenerateXsd(type!, argsOptions);
    }
}