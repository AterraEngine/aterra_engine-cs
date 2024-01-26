// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Contracts.Assets;

namespace AterraEngine.Assets;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class AssetNode(IAsset? asset = null, List<IAssetNode>? children = null) : IAssetNode {

    public IAsset? Asset { get; set; } = asset;
    public List<IAssetNode> Children { get; } = children ?? [];

    private List<IAsset>? _cachedFlat = null;
    public IEnumerable<IAsset> CachedFlat {
        get {
            if (_cachedFlat is not null) return _cachedFlat;
            _cachedFlat = Flat();
            return _cachedFlat;
        }
    }

    public List<IAsset> Flat() {
        List<IAsset> assets = [];
        if (Asset != null) assets.Add(Asset);
        assets.AddRange(Children.SelectMany(child => child.Flat()).ToList());
        return assets;
    }

    public int Count() {
        int count = Asset is null ? 0 : 1;
        count += Children.Sum(child => child.Count());
        return count;
    }
}