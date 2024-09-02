// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Attributes.ConfigMancer;
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.ConfigMancer;
using AterraCore.Contracts.FlexiPlug;
using JetBrains.Annotations;
using Serilog;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;

namespace AterraCore.ConfigMancer;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class ConfigMancerParser(IPluginAtlas pluginAtlas, ILogger logger) : IConfigMancerParser {
    private readonly ImmutableDictionary<string, ConfigMancerValueRecord> _parsableTypes = pluginAtlas.Plugins
        .SelectMany(plugin => plugin.Types)
        .SelectMany(type => type
            .GetCustomAttributes<ConfigMancerElementAttribute>()
            .Select(attribute => new ConfigMancerValueRecord(type, attribute, XmlRootElementName(type)))
        )
        .ToImmutableDictionary(record => record.RootName)
    ;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    private static string XmlRootElementName(Type type) => 
        type.GetCustomAttributes<XmlRootAttribute>().FirstOrDefault()?.ElementName 
        ?? char.ToLowerInvariant(type.Name[0]) + type.Name[1..];
    
    private static bool TryDeserializeWithAttributes(Type type, XmlNode node, [NotNullWhen(true)] out object? deserialized) {
        var serializer = new XmlSerializer(type, new XmlRootAttribute(node.LocalName) { Namespace = node.NamespaceURI });
        using var reader = new StringReader(node.OuterXml);
        deserialized = serializer.Deserialize(reader) ?? null; // If extracted, make a try catch, or a better TryDeserialize...
        return deserialized != null;
    }
    
    public bool TryParse(string filePath, out ParsedConfigs parsedConfigs) {
        parsedConfigs = default;
        var parsedObjects = new Dictionary<AssetId, object>();

        try {
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);

            if (xmlDoc.DocumentElement is null) {
                logger.Warning("Failed find any data in the XML file {path}", filePath);
                return false;
            }

            var queue = new Queue<XmlNode>(xmlDoc.DocumentElement.ChildNodes.Cast<XmlNode>());

            while (queue.TryDequeue(out XmlNode? node)) {
                if (_parsableTypes.TryGetValue(node.Name, out ConfigMancerValueRecord? record)
                    && TryDeserializeWithAttributes(record.Type, node, out object? deserialized)
                    && parsedObjects.TryAdd(record.ElementAttribute.AssetId, deserialized)
                ) continue;
                
                foreach (XmlNode n in node.ChildNodes) 
                    queue.Enqueue(n);
            }
            
            parsedConfigs = new ParsedConfigs(parsedObjects);
            return true;
        }
        catch (Exception ex) {
            logger.Error(ex, "Failed to parse XML file, {path}", filePath);
            return false;
        }
    }
}
