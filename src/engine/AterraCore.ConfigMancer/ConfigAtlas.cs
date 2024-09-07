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
public class ConfigAtlas(
    IConfigMancerParser configMancerParser,
    EngineConfigXml engineConfigXml,
    IPluginAtlas pluginAtlas
) : IConfigAtlas {
    public EngineConfigXml EngineConfigXml { get; } = engineConfigXml;
    public FrozenDictionary<PluginId, PluginConfigXml> PluginConfigXmls { get; } = pluginAtlas.Plugins.ToFrozenDictionary(
        keySelector: plugin => plugin.PluginId,
        elementSelector: plugin => plugin.PluginConfigXml
    );

    private IParsedConfigs? _gameConfigs;
    public IParsedConfigs GameConfigs {
        get {
            if (_gameConfigs is not null) return _gameConfigs ?? FrozenParsedConfigs.Empty;
            if (configMancerParser.TryParseGameConfig(EngineConfigXml.Paths?.GameConfigPath ?? Paths.ConfigGame, out FrozenParsedConfigs parsedConfigs)) return _gameConfigs ??= parsedConfigs;
            return _gameConfigs ??= FrozenParsedConfigs.Empty;
        }
    }
    public ParsedConfigs GameConfigsAsUnFrozen => ConvertInternalGameConfigToUnFrozen();
    public FrozenParsedConfigs GameConfigsAsFrozen => ConvertInternalGameConfigToFrozen();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    #region Converters between frozen & unfrozen
    public FrozenParsedConfigs ConvertInternalGameConfigToFrozen() {
        FrozenParsedConfigs configs = _gameConfigs switch {
            FrozenParsedConfigs frozenConfigs => frozenConfigs,
            ParsedConfigs parsedConfigs => new FrozenParsedConfigs(parsedConfigs.AsReadOnlyDictionary()),
            
            null => FrozenParsedConfigs.Empty,
            _ => FrozenParsedConfigs.Empty
        };
        
        _gameConfigs = configs;
        return configs;
    }
    
    public ParsedConfigs ConvertInternalGameConfigToUnFrozen() {
        ParsedConfigs configs = _gameConfigs switch {
            ParsedConfigs parsedConfigs => parsedConfigs,
            FrozenParsedConfigs frozenConfigs => new ParsedConfigs(frozenConfigs.AsReadOnlyDictionary()),
            
            null => ParsedConfigs.Empty,
            _ => ParsedConfigs.Empty
        };
        
        _gameConfigs = configs;
        return configs;
    }
    #endregion
    
}
