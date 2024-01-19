// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;
using AterraEngine_lib.structs;
using AterraEngine.Assets;
using AterraEngine.Assets.Lib;
using AterraEngine.Component;
using AterraEngine.Plugin;
using AterraEngine.Draw;
using AterraEngine.Interfaces.Assets;
using AterraEngine.Interfaces.Assets.Lib;
using AterraEngine.Interfaces.Component;
using AterraEngine.Interfaces.Draw;
using AterraEngine.Interfaces.Plugin;
using AterraEngine.Services;
using Microsoft.Extensions.DependencyInjection;
using Raylib_cs;

namespace EnginePlugin_Test;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class Plugin : EnginePlugin {
    public override string NameInternal { get; } = "ExamplePlugin";
    public override string NameReadable { get; } = "Example Plugin";

    public override void DefineServices(IServiceCollection serviceCollection) {
    }

    public override void DefineDataTextures() {
        ITextureAtlas textureAtlas = EngineServices.GetService<ITextureAtlas>();
        if (!textureAtlas.TryAddTexture("tDucky", "resources/DuckyHappy.png")) {
            throw new Exception("Texture could not be loaded");
        }
    }

    public override void DefineDataAssets() {
        IAssetAtlas assetAtlas = EngineServices.GetAssetAtlas();
        IWorld2D world2D = EngineServices.GetWorld2D();
        
        var PlayerComponent = new PlayerControllerLooking(new EngineAssetId(IdPrefix, NextInternalId())); // TODO we shouldn't do this like this.
        world2D.Level2D = new Level2D(new EngineAssetId(IdPrefix, NextInternalId()), "overworld");
        
        assetAtlas.TryAddAsset(world2D.Level2D);
        assetAtlas.TryAddAsset(PlayerComponent);
        
        ISpriteAtlas spriteAtlas = EngineServices.GetService<ISpriteAtlas>();
        
        if (!spriteAtlas.TryAddSprite("sDucky", "tDucky", out ISprite? sprite)) {
            throw new Exception("Sprite could not be added to atlas");
        }
        
        PlayerComponent.Sprite = sprite!;
        world2D.PlayerId = PlayerComponent.Id;

        const int v = 25;
        
        for (int i = -v; i < v; i++) {
            for (int j = -v; j < v; j++) {
                
                if (!spriteAtlas.TryAddSprite(
                        $"sDuckyBackground{i.ToString().PadLeft(3,'0')}{j.ToString().PadLeft(3,'0')}", "" +
                        "tDucky", 
                        out ISprite? spirteX)
                ) {
                    throw new Exception("Sprite could not be added to atlas -> inside loop");
                }

                EngineAssetId assetId = new EngineAssetId(IdPrefix, NextInternalId());
                Actor2DComponent actor2D = new Actor2DComponent(assetId, $"$actor{i}{j}")
                    {
                        Pos = new Vector2((spirteX!.Texture.Width / 4f)*i, (spirteX!.Texture.Height / 4f)*j),
                        Rotation = (i * j) * 0.1f,
                        Sprite = spirteX
                    };

                assetAtlas.TryAddAsset(actor2D);
                world2D.Level2D.Assets.Add(assetId);
            }
        }
        // TODO This shouldn't be the case, the fix should not be to add the player as the last asset
        world2D.Level2D.Assets.Add(PlayerComponent.Id);

    }
    
}