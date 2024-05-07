// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;
using AterraCore.Common.Config;
using AterraCore.FlexiPlug.Config;
using AterraEngine.Config;
using CliArgsParser.Attributes;
using CliArgsParser.Contracts;
using JetBrains.Annotations;
using Serilog;
using Xml;
using Xml.Contracts;

namespace ProductionTools.Commands;

// ---------------------------------------------------------------------------------------------------------------------
// Support Code
// ---------------------------------------------------------------------------------------------------------------------
[SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
public class ArgsOptions : IParameters {
    [AutoArgValue("classname")] public string ClassName { get; set; } = null!;
    [AutoArgFlag("prettify")]public bool Prettify { get; set; } = true;
    [AutoArgValue("output")] public string? OutputFile { get; set; }
    [AutoArgValue("folder")] public string? OutputFolder { get; set; }
}


public record XsdGeneratorRecord(
    IXsdGenerator Generator,
    string NameSpace
);

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[CommandAtlas]
public class XmlSchemaGenerator(ILogger logger) {
    private readonly Dictionary<string, XsdGeneratorRecord> _dictionary = new() {
        { "engine-config", new XsdGeneratorRecord(new XsdGenerator<EngineConfigXml>(logger), XmlNameSpaces.ConfigEngine) },
        { "plugin-config", new XsdGeneratorRecord(new XsdGenerator<PluginConfigXml>(logger), XmlNameSpaces.ConfigPlugin) },
    };
    
    // -----------------------------------------------------------------------------------------------------------------
    // Commands
    // -----------------------------------------------------------------------------------------------------------------
    [Command<ArgsOptions>("generate-xmlschema")]
    [UsedImplicitly]
    public void GenerateXmlSchemaEngineConfig(ArgsOptions argsOptions) {
        string className = argsOptions.ClassName.ToLowerInvariant();
        
        if (!_dictionary.TryGetValue(className, out XsdGeneratorRecord? record)) {
            logger.Warning("No match found for the provided ClassName: {ClassName}. Check the ClassName and try again.", className);
            return;
        }
        
        string outputPath = Path.Combine(
            argsOptions.OutputFolder ?? string.Empty, 
            argsOptions.OutputFile ?? $"{className}.xsd");

        logger.Information("Generating XML Schema for {ClassName} with output path {Path}.",
            className, outputPath);
        
        record.Generator.GenerateXsd(
            record.NameSpace,
            argsOptions.Prettify,
            outputPath
        );

        logger.Information("XML Schema generated successfully");
    }
}