// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Collections;
using OldAterraEngine.Contracts.Assets;

namespace OldAterraEngine.Lib.Actors;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class AssetNodeRoot(List<IAssetNode>? children = null) : IAssetNodeRoot {
    public List<IAssetNode> Children { get; } = children ?? [];
    private List<IAsset>? _cachedFlat;
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public IEnumerable<IAsset> CachedFlat {
        get {
            if (_cachedFlat is not null) 
                return _cachedFlat;
        
            _cachedFlat = Flat().ToList();
            return _cachedFlat;
        }
    }

    public IEnumerable<IAsset> Flat() {
        var stack = new Stack<IAssetNode>(Children);
        
        while(stack.Count != 0) {
            IAssetNode? node = stack.Pop();

            if (node.Asset != null) 
                yield return node.Asset;

            foreach (IAssetNode? child in node.Children)
                stack.Push(child);
        }
    }

    public int Count() => CachedFlat.Count();

    public IEnumerator<IAsset> GetEnumerator() {
        return CachedFlat.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }
}