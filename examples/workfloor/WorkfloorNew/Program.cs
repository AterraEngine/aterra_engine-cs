// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Contracts.PluginFramework;
using AterraEngine.Core.PluginFramework;
using AterraEngine.Core.ServicesFramework;
using AterraEngine.Core.Types;
using AterraEngine.Lib.Plugin;
using Microsoft.Extensions.DependencyInjection;

namespace WorkfloorNew;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
class PluginTestServices : PluginServicesFactory {
    public override void LoadServices(IServiceCollection serviceCollection) {
        
    }
}

class PluginTestAssets : PluginAssetsFactory {
    public override void LoadAssets() {
        
    }
}

class PluginTestTextures : PluginTexturesFactory {
    public override void LoadTextures() {
        
    }
}

class PluginTest(PluginId id) : Plugin(id) {
    public override IPluginServicesFactory? Services { get; } = new PluginTestServices();
    public override IPluginAssetsFactory    Assets            => EngineServices.CreateWithServices<PluginTestAssets>();
    public override IPluginTexturesFactory  Textures          => EngineServices.CreateWithServices<PluginTestTextures>();
}

static class Program {
    public static void Main(string[] args) {
        var pluginFactory = new PluginFactory(true);
        pluginFactory.TryLoadPluginFromType(typeof(PluginTest));
        pluginFactory.TryLoadPluginFromType(typeof(PluginTest));
        pluginFactory.TryLoadPluginFromType(typeof(PluginTest));
        pluginFactory.TryLoadPluginFromType(typeof(PluginTest));

        var serviceCollection = new ServiceCollection();
        var pluginLoader = new PluginLoader(serviceCollection);
        pluginLoader.LoadPlugins(pluginFactory.Plugins);
    }
}