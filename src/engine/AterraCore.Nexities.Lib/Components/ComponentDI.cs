// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraCore.Nexities.Lib.Components;

using Common.Nexities;
using Contracts.Nexities.Data.Assets;
using Contracts.Nexities.Data.Components.AssetTree;
using HUD.Text;
using Microsoft.Extensions.DependencyInjection;
using Sprite2D;
using Transform2D;
using Transform3D;

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
