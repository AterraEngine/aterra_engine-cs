// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using AterraCore.Contracts.AtlasHub;
using AterraCore.Contracts.FlexiPlug;
using AterraCore.Contracts.Nexities;
using AterraCore.FlexiPlug.Assets;
using AterraCore.Types;
using Guid = System.Guid;

namespace AterraCore.AtlasHub;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class InstanceAtlas(IAssetAtlas assetAtlas) : IInstanceAtlas {
    private readonly ConcurrentDictionary<Guid, IAsset> _dictionary = new();
    public IReadOnlyDictionary<Guid, IAsset> Dictionary => _dictionary.AsReadOnly();

    // ---------------------------------------------------------------------------------------------------------------------
    // Methods
    // ---------------------------------------------------------------------------------------------------------------------
    
    
    public bool TryCreateInstance<T, TDto>(AssetId assetId,TDto assetDto, [NotNullWhen(true)] out T? asset)
        where T : IAsset
        where TDto : IAssetDto {

        asset = default;
        if (TryCreateInstance(assetId, assetDto, out IAsset? a)) return false;
        asset = (T)a!;
        return true;
    }
    
    public bool TryCreateInstance<TDto>(AssetId assetId, TDto assetDto, [NotNullWhen(true)] out IAsset? asset) 
        where TDto : IAssetDto {
        
        asset = null;
        if (!assetAtlas.TryGetAssetRecord(assetId, out Type? type)) return false;

        try {
            asset = (IAsset)Activator.CreateInstance(type, [assetDto])!;
            return _dictionary.TryAdd(asset.Guid, asset);
        }
        catch (MissingMethodException) {
            return false;
        }
    }


    public bool TryRemoveInstance(Guid instanceId) {
        return _dictionary.TryRemove(instanceId, out _);
    }
}