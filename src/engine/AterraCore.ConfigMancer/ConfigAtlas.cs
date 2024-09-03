// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Attributes;
using AterraCore.Common.ConfigFiles;
using AterraCore.Common.Data;
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.ConfigMancer;
using AterraCore.Contracts.FlexiPlug;
using JetBrains.Annotations;
using System.Collections.Frozen;

namespace AterraCore.ConfigMancer;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
[Injectable<ConfigAtlas, IConfigAtlas>]
public class ConfigAtlas(IConfigMancerParser configMancerParser, EngineConfigXml engineConfigXml, IPluginAtlas pluginAtlas) : IConfigAtlas {
    public EngineConfigXml EngineConfigXml { get; } = engineConfigXml;
    public FrozenDictionary<PluginId, PluginConfigXml> PluginConfigXmls { get; } = pluginAtlas.Plugins.ToFrozenDictionary(
        plugin => plugin.PluginId,
        plugin => plugin.PluginConfigXml
    );
    
    private ParsedConfigs? _gameConfigs;
    public ParsedConfigs GameConfigs {
        get {
            if (_gameConfigs is not null) return _gameConfigs ?? ParsedConfigs.Empty;
            if (configMancerParser.TryParseGameConfig(EngineConfigXml.Paths?.GameConfigPath ?? Paths.ConfigGame, out ParsedConfigs parsedConfigs)) return _gameConfigs ??= parsedConfigs;
            return _gameConfigs ??= ParsedConfigs.Empty;
        }
    }
    
    // Todo add a feature to add new configs to the ParsedConfig, based on the plugins that are loaded
    //      Issue with this is how to handle plugins that have updated themselves
}
