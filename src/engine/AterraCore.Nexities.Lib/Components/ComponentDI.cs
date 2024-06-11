// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Nexities.Data.Assets;
using AterraCore.Contracts.Nexities.Data.Components.AssetTree;
using AterraCore.Nexities.Lib.Components.HUD.Text;
using AterraCore.Nexities.Lib.Components.Sprite2D;
using AterraCore.Nexities.Lib.Components.Transform2D;
using AterraCore.Nexities.Lib.Components.Transform3D;
using Microsoft.Extensions.DependencyInjection;

namespace AterraCore.Nexities.Lib.Components;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public static class ComponentDi {
    public static IServiceCollection AddComponentFactories(this IServiceCollection serviceCollection) {
        serviceCollection.AddTransient(GetRequiredAssetInstance<ITransform2D>);
        serviceCollection.AddTransient(GetRequiredAssetInstance<ITransform3D>);
        serviceCollection.AddTransient(GetRequiredAssetInstance<IAssetTree>);
        serviceCollection.AddTransient(GetRequiredAssetInstance<IRaylibHudText>);
        serviceCollection.AddTransient(GetRequiredAssetInstance<ISprite2D>);

        return serviceCollection;
    }

    private static T GetRequiredAssetInstance<T>(IServiceProvider provider) where T : IAssetInstance {
        var instanceAtlas = provider.GetRequiredService<IAssetInstanceAtlas>();
        var assetAtlas = provider.GetRequiredService<IAssetAtlas>();

        if (!assetAtlas.TryGetAssetId<T>(out AssetId assetId)) throw new InvalidOperationException();
        if (!instanceAtlas.TryCreateInstance(assetId, out T? instance)) throw new InvalidOperationException();

        return instance;
    }
}
