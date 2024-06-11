// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Data;
using AterraCore.Common.Types.FlexiPlug;
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.FlexiPlug;
using AterraCore.Contracts.FlexiPlug.Plugin;
using AterraCore.Contracts.Nexities.Data.Assets;
using AterraCore.Contracts.Nexities.Data.Components;
using AterraCore.Contracts.Nexities.Data.Entities;
using AterraCore.Contracts.Nexities.DataParsing;
using AterraCore.Contracts.Nexities.DataParsing.NamedValues;
using AterraCore.Nexities.Parsers.FileElements;
using AterraCore.Nexities.Parsers.NamedValues;
using Serilog;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace AterraCore.Nexities.Parsers;

using IComponent=IComponent;

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

    public bool TryLoadInstancesFromFile(string filePath) {
        if (!TryParse(filePath, out AssetDataXml? assetData)) {
            logger.Warning("File {filePath} could not be loaded", filePath);
            return false;
        }

        // Think of how we can cache this
        Dictionary<string, PluginId> pluginMap = assetData.RequiredPlugins
            .Where(dto => dto.ReadableName is not null)
            .Select(dto => {
                bool result = pluginAtlas.TryGetPluginByReadableName(dto.ReadableName!, out IPluginRecord? plugin);
                return new {
                    Dto = dto,
                    IsKnown = result,
                    PluginId = plugin?.Id
                };
            })
            .Where(box => box.IsKnown)
            .ToDictionary(
            // Because IsKnown value already marks if the plugins could be mapped 
            keySelector: box => box.Dto.InternalRefId!,
            elementSelector: box => (PluginId)box.PluginId!
            );

        foreach (AssetDto asset in assetData.Assets) {
            // Try and get the plugin ID, necessary to find actual asset Id
            if (!TryGetAssetId(pluginMap, asset.RawAssetId!, out AssetId assetId)) {
                logger.Warning("Asset could not be loaded");// TODO add file, guid if present etc ...
                continue;
            }

            // Create the instance
            if (!instanceAtlas.TryCreateInstance(assetId, out INexitiesEntity? instance, asset.Guid)) {
                logger.Warning("Asset could not be instantiated");
                continue;
            }

            // Populate the instance with component data
            foreach (ComponentDto componentDto in asset.Components) {
                if (!TryGetAssetId(pluginMap, componentDto.RawAssetId!, out AssetId componentId)) {
                    logger.Warning("Component could not be loaded");// TODO add file, guid if present etc ...
                    continue;
                }

                IComponent? foundComponent = instance.Components.FirstOrDefault(c => c.AssetId == componentId);
                if (foundComponent is null) {
                    logger.Warning("Component could not be loaded");// TODO add file, guid if present etc ...
                    continue;
                }

                // TODO make this not happen here
                var namedValueProperties = foundComponent
                        .GetType()
                        .GetProperties()
                        .Select(property => new {
                            Property = property,
                            Attribute = property.GetCustomAttribute<NamedValueAttribute>(true) ?? null
                        })
                        .Where(box => box.Attribute != null)
                        .ToDictionary(
                        keySelector: box => box.Attribute!.Name!,
                        elementSelector: box => box
                        )
                    ;

                foreach (ValueDto componentDtoValue in componentDto.Values) {
                    if (!namedValueProperties.TryGetValue(componentDtoValue.Name!, out var box)) {
                        logger.Warning("Component could not be loaded");// TODO add file, guid if present etc ...
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

    // -----------------------------------------------------------------------------------------------------------------
    // Helper Methods
    // -----------------------------------------------------------------------------------------------------------------
    private bool TryGetAssetId(Dictionary<string, PluginId> pluginMap, string rawAssetId, out AssetId assetId) {
        assetId = default;

        string[] split = rawAssetId.Split(":");
        if (split.Length != 2) return false;
        if (!pluginMap.TryGetValue(split[0], out PluginId pluginId)) return false;
        if (!PartialAssetId.TryParse(split[1], out PartialAssetId partialAssetId)) return false;

        assetId = new AssetId(pluginId, partialAssetId);
        return true;
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public bool TryParse(string filePath, [NotNullWhen(true)] out AssetDataXml? assetData) => Parser.TryDeserializeFromFile(filePath, out assetData);
}
