// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraCore.Nexities.Parsers;

using Common.Data;
using Common.Types.FlexiPlug;
using Common.Types.Nexities;
using Contracts.FlexiPlug;
using Contracts.FlexiPlug.Plugin;
using Contracts.Nexities.Data.Assets;
using Contracts.Nexities.Data.Entities;
using Contracts.Nexities.DataParsing;
using Contracts.Nexities.DataParsing.NamedValues;
using FileElements;
using NamedValues;
using Serilog;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using IComponent=Contracts.Nexities.Data.Components.IComponent;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class AssetDataXmlService(
    ILogger logger, 
    IPluginAtlas pluginAtlas,
    IAssetInstanceAtlas instanceAtlas, 
    INamedValueConverter namedValueConverter
) : IAssetDataXmlService {
    
    private AssetDataXmlParser Parser { get; } = new(logger, XmlNameSpaces.AssetData, Paths.Xsd.XsdAssetDataDto);
    
    // -----------------------------------------------------------------------------------------------------------------
    // Helper Methods
    // -----------------------------------------------------------------------------------------------------------------
    private bool TryGetAssetId(Dictionary<string, PluginId> pluginMap, string rawAssetId, out AssetId assetId) {
        assetId = default;
        
        string[] split = rawAssetId.Split(":");
        if (split.Length != 2) return false;
        if (!pluginMap.TryGetValue( split[0], out PluginId pluginId)) return false;
        if (!PartialAssetId.TryParse(split[1], out PartialAssetId partialAssetId)) return false;

        assetId = new AssetId(pluginId, partialAssetId);
        return true;
    }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public bool TryParse(string filePath, [NotNullWhen(true)] out AssetDataXml? assetData) {
        return Parser.TryDeserializeFromFile(filePath, out assetData);
    }

    public bool TryLoadInstancesFromFile(string filePath) {
        if (!TryParse(filePath, out AssetDataXml? assetData)) {
            logger.Warning("File {filePath} could not be loaded", filePath);
            return false;
        }
        
        // Think of how we can cache this
        Dictionary<string, PluginId> pluginMap = assetData.RequiredPlugins
            .Where(dto => dto.ReadableName is not null)
            .Select(dto => {
                bool result = pluginAtlas.TryGetPluginByReadableName(dto.ReadableName!, out IPlugin? plugin);
                return new {
                    Dto = dto,
                    IsKnown = result,
                    PluginId = plugin?.Id
                };
            })
            .Where(box => box.IsKnown)
            .ToDictionary(
                // Because IsKnown value already marks if the plugins could be mapped 
                box => box.Dto.InternalRefId!,
                box => (PluginId)box.PluginId!
            );
        
        foreach (AssetDto asset in assetData.Assets) {
            // Try and get the plugin ID, necessary to find actual asset Id
            if (!TryGetAssetId(pluginMap, asset.RawAssetId!, out AssetId assetId)) {
                logger.Warning("Asset could not be loaded"); // TODO add file, guid if present etc ...
                continue;
            }

            // Create the instance
            if (!instanceAtlas.TryCreateInstance(assetId, out INexitiesEntity? instance, predefinedGuid:asset.DefinedGuid)) {
                logger.Warning("Asset could not be instantiated");
                continue;
            }
            
            // Populate the instance with component data
            foreach (ComponentDto componentDto in asset.Components) {
                if (!TryGetAssetId(pluginMap, componentDto.RawAssetId!, out AssetId componentId)) {
                    logger.Warning("Component could not be loaded"); // TODO add file, guid if present etc ...
                    continue;
                }

                IComponent? foundComponent = instance.Components.FirstOrDefault(c => c.AssetId == componentId);
                if (foundComponent is null) {
                    logger.Warning("Component could not be loaded"); // TODO add file, guid if present etc ...
                    continue;
                }

                var namedValueProperties = foundComponent
                    .GetType()
                    .GetProperties()
                    .Select(property => new {
                        Property = property,
                        Attribute = property.GetCustomAttribute<NamedValueAttribute>(true) ?? null
                    })
                    .Where(box => box.Attribute != null)
                    .ToDictionary(
                        box => box.Attribute!.Name!,
                        box => box
                    )
                ;

                foreach (ValueDto componentDtoValue in componentDto.Values) {
                    if (!namedValueProperties.TryGetValue(componentDtoValue.Name!, out var box)) {
                        logger.Warning("Component could not be loaded"); // TODO add file, guid if present etc ...
                        continue;
                    }
                    
                    box.Property.SetValue(
                        foundComponent,
                        namedValueConverter.GetProcessor(box.Attribute!.Convertor).DynamicInvoke(componentDtoValue.Value)
                    );
                }
                
                
            }
            
        }

        return true;
    }
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
}
