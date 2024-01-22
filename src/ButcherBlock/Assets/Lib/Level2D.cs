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
    
    public override void Draw(Vector2 worldToScreenSpace) {
        foreach (IAsset asset in _loadedAssets) {
            asset.Draw(worldToScreenSpace);
        }
    }
    
    public override void DrawDebug(Vector2 worldToScreenSpace) {
        for (int i = 0; i < _loadedAssets.Length; i++) {
            _loadedAssets[i].DrawDebug(worldToScreenSpace);
        }
    }
    
    public void CollideAll() {
        int count = _loadedAssets.Length;
        int compared = 0;
        List<IActor2DComponent> removeComps = [];

        // Get the distance all of them, and only check those which are within a certain distance of each other
        
        for (int i = 0; i < count - 1; i++) {
            for (int j = i + 1; j < count; j++) {
                var box1 = ((IActor2DComponent)_loadedAssets[i]).Box;
                var box2 = ((IActor2DComponent)_loadedAssets[j]).Box;
                //
                // var closestX = Math.Max(box1.X, Math.Min(box2.X, box1.X + box1.Width));
                // var closestY = Math.Max(box1.Y, Math.Min(box2.Y, box1.Y + box1.Height));
                //
                // var distance = Math.Sqrt(Math.Pow(box2.X - closestX, 2) + Math.Pow(box2.Y - closestY, 2));
                //
                // if (distance <= 0) {
                //     removeComps.Add((IActor2DComponent)_loadedAssets[i]);
                // }
                
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