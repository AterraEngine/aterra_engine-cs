// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Contracts.Core.PluginFramework;
using AterraEngine.Core.ServicesFramework;
using AterraEngine.Core.Types;

namespace Workfloor.Plugin;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
class PluginTest(PluginId id) : AterraEngine.Lib.Plugin.Plugin(id) {
    public override IPluginServiceBuilder? Services { get; } = new PluginTestServices();
    public override IPluginAssetsBuilder    Assets            => EngineServices.CreateWithServices<PluginTestAssets>();
    public override IPluginTexturesBuilder  Textures          => EngineServices.CreateWithServices<PluginTestTextures>();
}