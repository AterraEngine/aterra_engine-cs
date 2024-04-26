// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using AterraCore.Config;
using AterraCore.Config.EngineConfig;
using AterraCore.Config.PluginConfig;
using AterraCore.Contracts.Config;
using CliArgsParser;
using CliArgsParser.Attributes;
using JetBrains.Annotations;
using Serilog;

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
    [ArgValue('n', "namespace-prefix")] public string NamespacePrefix { get; set; } = "aterra-engine";
}

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class XmlSchemaGenerator(ILogger logger) : CliCommandAtlas {
    private readonly Dictionary<string, IXsdGenerator> _dictionary = new() {
        { "engine-config", new XsdGenerator<EngineConfigDto>(logger) },
        { "plugin-config", new XsdGenerator<PluginConfigDto>(logger) },
    };
    
    // -----------------------------------------------------------------------------------------------------------------
    // Commands
    // -----------------------------------------------------------------------------------------------------------------
    [CliCommand<ArgsOptions>("generate-xmlschema")]
    [UsedImplicitly]
    public void GenerateXmlSchemaEngineConfig(ArgsOptions argsOptions) {
        string className = argsOptions.ClassName.ToLowerInvariant();
        
        if (!_dictionary.TryGetValue(className, out IXsdGenerator? xsdGenerator)) {
            logger.Warning("No match found for the provided ClassName: {ClassName}. Check the ClassName and try again.", className);
            return;
        }
        
        string outputPath = Path.Combine(
            argsOptions.OutputFolder ?? string.Empty, 
            argsOptions.OutputFile ?? $"{className}.xsd");

        logger.Information("Generating XML Schema for {ClassName} with output path {Path}.",
            className, outputPath);
        
        xsdGenerator.GenerateXsd(
            $"urn:{argsOptions.NamespacePrefix}:{className}",
            argsOptions.Prettify,
            outputPath
        );

        logger.Information("XML Schema generated successfully");
    }
}