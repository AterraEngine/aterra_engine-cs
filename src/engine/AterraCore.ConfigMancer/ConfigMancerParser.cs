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
        .SelectMany(plugin => plugin.Types
            .SelectMany(type => type
                .GetCustomAttributes<ConfigMancerElementAttribute>()
                .Select( attribute => (
                    type,
                    attribute, 
                    rootName:type.GetCustomAttributes<XmlRootAttribute>().FirstOrDefault()?.ElementName
                             ?? char.ToLowerInvariant(type.Name[0]) + type.Name[1..]
                    )
                )
            )
        )
        .ToImmutableDictionary(
            tuple => tuple.rootName,
            tuple => new ConfigMancerValueRecord(tuple.type, tuple.attribute, tuple.rootName)
        )
    ;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    private static bool TryDeserializeWithAttributes(Type type, XmlNode node, [NotNullWhen(true)] out dynamic? deserialized) {
        var serializer = new XmlSerializer(type, new XmlRootAttribute(node.LocalName) { Namespace = node.NamespaceURI });
        using var reader = new StringReader(node.OuterXml);
        deserialized = serializer.Deserialize(reader) ?? null;
        return deserialized != null;
    }
    
    public bool TryParse(string filePath, out ParsedConfigs parsedConfigs) {
        parsedConfigs = default;
        var parsedObjects = new Dictionary<AssetId, dynamic>();
        
        logger.Debug("{t}", _parsableTypes);

        try {
            using var reader = new StreamReader(filePath);
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(reader);

            if (xmlDoc.DocumentElement is null) return false;

            var queue = new Queue<XmlNode>();
            foreach (XmlNode node in xmlDoc.DocumentElement.ChildNodes) {
                queue.Enqueue(node);
            }

            while (queue.TryDequeue(out XmlNode? node)) {
                if (_parsableTypes.TryGetValue(node.Name, out ConfigMancerValueRecord? record)
                    && TryDeserializeWithAttributes(record.Type, node, out dynamic? deserialized)
                    && parsedObjects.TryAdd(record.ElementAttribute.AssetId, deserialized)
                ) {
                    logger.Debug("{type} parsed", record?.Type);
                    continue;
                }
            
                foreach (XmlNode n in node.ChildNodes) {
                    queue.Enqueue(n);
                }
            }
            
            parsedConfigs = new ParsedConfigs(parsedObjects);
            return true;
        }
        catch (Exception e) {
            logger.Warning(e, "Failed to parse XML file");
            return false;
        }
    }
}
