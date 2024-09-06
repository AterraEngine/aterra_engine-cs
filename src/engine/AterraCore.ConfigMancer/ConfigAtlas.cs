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
[Singleton<ConfigAtlas, IConfigAtlas>]
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
}
