// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.ConfigFiles;
using AterraCore.Common.Types.Nexities;
using System.Collections.Frozen;

namespace AterraCore.Contracts.ConfigMancer;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IConfigAtlas {
    EngineConfigXml EngineConfigXml { get; }
    FrozenDictionary<PluginId, PluginConfigXml> PluginConfigXmls { get; }
    ParsedConfigs GameConfigs { get; }
}
