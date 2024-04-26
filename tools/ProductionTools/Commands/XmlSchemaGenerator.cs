// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using AterraCore.Config;
using AterraCore.Config.EngineConfig;
using AterraCore.Config.PluginConfig;
using AterraCore.Contracts.Config;
using CliArgsParser;
using CliArgsParser.Attributes;
using JetBrains.Annotations;
using Serilog.Core;

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
    private readonly IXsdGenerator _xsdGenerator = new XsdGenerator();
    private readonly Dictionary<string, Type> _dictionary = new() {
        { "engine-config", typeof(EngineConfigDto) },
        { "plugin-config", typeof(PluginConfigDto) },
    };
    
    // -----------------------------------------------------------------------------------------------------------------
    // Commands
    // -----------------------------------------------------------------------------------------------------------------
    [CliCommand<ArgsOptions>("generate-xmlschema")]
    [UsedImplicitly]
    public void GenerateXmlSchemaEngineConfig(ArgsOptions argsOptions) {
        string className = argsOptions.ClassName.ToLowerInvariant();
        
        if (!_dictionary.TryGetValue(className, out Type? type)) {
            Console.WriteLine("No match found for the provided ClassName. Check the ClassName and try again.");
        }
        
        _xsdGenerator.GenerateXsd(
            type!,
            $"urn:aterra-engine:{className}",
            argsOptions.Prettify,
            Path.Combine(
                argsOptions.OutputFolder ?? string.Empty, 
                argsOptions.OutputFile ?? $"{className}.xsd"
            )
        );
    }
}