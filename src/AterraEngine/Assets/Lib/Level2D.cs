// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;
using AterraEngine_lib.structs;
using AterraEngine.Component;
using AterraEngine.Interfaces.Assets;
using AterraEngine.Interfaces.Assets.Lib;
using AterraEngine.Interfaces.Component;
using AterraEngine.Services;
using Raylib_cs;

namespace AterraEngine.Assets.Lib;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class Level2D(EngineAssetId id, string? internalName) : Asset(id, internalName), ILevel2D {
    public List<EngineAssetId> Assets { get; set; } = [];
    private IAsset[] _loadedAssets = [];

    public void ResolveAssetIds() {
        IAssetAtlas assetAtlas = EngineServices.GetAssetAtlas();

        _loadedAssets = Assets.Select(assetId => { 
            assetAtlas.TryGetAsset(assetId, out IAsset? asset);
            return asset;
        }).ToArray()!;
    }
    
    public override void Draw() {
        foreach (IAsset asset in _loadedAssets) {
            asset.Draw();
        }
    }
    
    public override void DrawDebug() {
        for (int i = 0; i < _loadedAssets.Length; i++) {
            _loadedAssets[i].DrawDebug();
        }
    }
    
    public void CollideAll() {
        int count = _loadedAssets.Length;
        int compared = 0;
        List<IActor2DComponent> removeComps = [];

        for (int i = 0; i < count - 1; i++) {
            for (int j = i + 1; j < count; j++) {
                var box1 = ((IActor2DComponent)_loadedAssets[i]).Box;
                var box2 = ((IActor2DComponent)_loadedAssets[j]).Box;
                
                // Check for collision between DrawableComponents[i] and DrawableComponents[j]
                if (Raylib.CheckCollisionRecs(box1, box2) &&  _loadedAssets[i].Id != EngineServices.GetWorld2D().PlayerId) {
                    removeComps.Add((IActor2DComponent)_loadedAssets[i]);
                }

                compared++;
            }
        }

        foreach (var comp in removeComps) {
            _loadedAssets = _loadedAssets.Where(c => c != comp).ToArray();
        }
        
        Console.WriteLine(compared);
    }
}