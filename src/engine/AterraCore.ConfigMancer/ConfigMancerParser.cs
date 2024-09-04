// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Attributes;
using AterraCore.Common.ConfigFiles;
using AterraCore.Common.Data;
using AterraCore.Contracts.ConfigMancer;
using AterraCore.Contracts.FlexiPlug;
using AterraCore.Contracts.PoolCorps;
using JetBrains.Annotations;
using Serilog;
using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using Xml;

namespace AterraCore.ConfigMancer;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
[Injectable<IConfigMancerParser>]
public class ConfigMancerParser(IPluginAtlas pluginAtlas, ILogger logger, IXmlPools xmlPools) : IConfigMancerParser {
    private readonly FrozenDictionary<string, ConfigMancerValueRecord> _parsableTypes = pluginAtlas.Plugins
        .SelectMany(plugin => plugin.Types)
        .Where(type => typeof(IConfigMancerElement).IsAssignableFrom(type) && type is { IsClass: true, IsAbstract: false })
        .Select(type =>  new ConfigMancerValueRecord(type, XmlRootElementName(type), type.GetCustomAttributes<XmlRootAttribute>().First()))
        .ToFrozenDictionary(record => record.RootName)
    ;
    
    private XmlParser<GameConfigXml>? _gameConfigParser;
    private XmlParser<GameConfigXml> GameConfigParser => _gameConfigParser ??= new XmlParser<GameConfigXml>(logger, XmlNameSpaces.ConfigGame, Paths.Xsd.XsdGameConfigDto );
    
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
    
    public bool TryParseGameConfig(string filePath, out ParsedConfigs parsedConfigs) {
        parsedConfigs = default;
        var parsedObjects = new Dictionary<Type, object>();
        Queue<XmlNode> queue = xmlPools.XmlNodeQueuePool.Get();

        try {
            if (!GameConfigParser.TryDeserializeFromFile(filePath, out GameConfigXml? configDto)) {
                logger.Warning("Failed find any data in the XML file {path}", filePath);
                return false;
            }
            
            foreach (XmlNode node in configDto.Configs) {
                queue.Enqueue(node);
            }

            while (queue.TryDequeue(out XmlNode? node)) {
                if (_parsableTypes.TryGetValue(node.Name, out ConfigMancerValueRecord? record) 
                    && TryDeserializeWithAttributes(record.Type, node, out object? deserialized)
                    && parsedObjects.TryAdd(record.Type, deserialized)
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
        finally {
            xmlPools.XmlNodeQueuePool.Return(queue);
        }
    }

}
