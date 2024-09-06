// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.ConfigFiles;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using Xml;
using AterraCore.Common.Data;
using AterraLib;

namespace ProductionTools.Commands;
// ---------------------------------------------------------------------------------------------------------------------
// Support Code
// ---------------------------------------------------------------------------------------------------------------------
[SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
public class ArgsOptions : ICommandParameters {
    [ArgValue("classname")] public string ClassName { get; set; } = null!;
    [ArgFlag("prettify")] public bool Prettify { get; set; } = true;
    [ArgValue("output")] public string? OutputFile { get; set; }
    [ArgValue("folder")] public string? OutputFolder { get; set; }
}

public record XsdGeneratorRecord(
    string NameSpace,
    Type Type
);

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class XmlSchemaGenerator(ILogger logger) : ICommandAtlas {
    private readonly XsdGenerator _xsdGenerator = new(logger);

    private readonly Dictionary<string, XsdGeneratorRecord> _dictionary = new() {
        { "engine-config", new XsdGeneratorRecord(XmlNameSpaces.ConfigEngine, typeof(EngineConfigXml)) },
        { "plugin-config", new XsdGeneratorRecord(XmlNameSpaces.ConfigPlugin, typeof(PluginConfigXml)) },
        { "game-config", new XsdGeneratorRecord(XmlNameSpaces.ConfigGame, typeof(GameConfigXml)) },
        { "game-config-aterralib", new XsdGeneratorRecord(XmlNameSpaces.ConfigGameAterraLib, typeof(AterraLibGameConfig)) }
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

        _xsdGenerator.GenerateXsd(
            record.Type,
            record.NameSpace,
            argsOptions.Prettify,
            outputPath
        );

        logger.Information("XML Schema generated successfully");
    }
}
