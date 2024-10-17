// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Attributes.DI;
using AterraCore.Common.ConfigFiles;
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Boot;
using AterraCore.Contracts.Boot.Logic.PluginLoading.Dto;
using AterraCore.Contracts.FlexiPlug;
using AterraCore.Contracts.FlexiPlug.Plugin;
using JetBrains.Annotations;
using System.Collections.Frozen;
using System.Collections.Immutable;

namespace AterraCore.FlexiPlug;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Singleton<IPluginAtlasFactory>]
[UsedImplicitly]
public class PluginAtlasFactory(IServiceProvider provider, IBootComponents bootComponents) : IPluginAtlasFactory {

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public IPluginAtlas GetAtlas() {
        LinkedList<IPluginRecord> plugins = new();
        HashSet<PluginId> pluginIds = [];

        foreach (IPluginBootDto dto in bootComponents.ValidPlugins) {
            plugins.AddLast(new PluginRecord {
                PluginId = dto.PluginNameSpaceId,
                Types = dto.Types,
                PluginBootDto = dto,
                PluginConfigXml = dto.TryGetPluginConfig(out PluginConfigXml? config)
                    ? config
                    : new PluginConfigXml()
            });

            pluginIds.Add(dto.PluginNameSpaceId);
        }

        ImmutableArray<PluginId> pluginIdsByOrder = [..plugins.Select(record => record.PluginId)];

        var pluginAtlas = new PluginAtlas(provider) {
            PluginIdsByOrder = pluginIdsByOrder,
            Plugins = plugins,
            PluginIds = pluginIds.ToFrozenSet()
        };

        return pluginAtlas;
    }
}
