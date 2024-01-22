// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using AterraEngine_lib.structs;
using AterraEngine.Interfaces.Assets;

namespace AterraEngine.Assets;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class AssetAtlas:IAssetAtlas {
    private readonly Dictionary<EngineAssetId, IAsset> _assets = new();
    public ReadOnlyDictionary<EngineAssetId, IAsset> Assets => _assets.AsReadOnly();
    
    private static readonly Regex _rxEngineAssetId = new("^([0-9a-fA-F]{4})([0-9a-fA-F]{8})$"); // 1234FEDCBA98
    
    public bool TryGetAsset(EngineAssetId value, out IAsset? asset) {
        return _assets.TryGetValue(value, out asset);
    }
    
    public bool TryGetAsset(string value, out IAsset? asset) {
        if (TryParseAssetIdFromString(value: value, out var engineAssetId) 
            && engineAssetId != null
        ) return TryGetAsset((EngineAssetId)engineAssetId, out asset);
        
        asset = null;
        return false;
    }
    
    
    public bool TryParseAssetIdFromString(string value, out EngineAssetId? engineAssetId) {
        Match match = _rxEngineAssetId.Match(value.PadLeft(12, '0')); // PAD to correct length
        if (!match.Success) {
            engineAssetId = null;
        } else {
            engineAssetId = new EngineAssetId(
                // Don't use Group0 as it is the entire matched string
                new PluginId(match.Groups[1].Value),
                int.Parse(match.Groups[2].Value, System.Globalization.NumberStyles.HexNumber)
            );
        }
        return match.Success;
    }


    public bool TryAddAsset(IAsset asset) {
        return _assets.TryAdd(asset.Id, asset);
    }
    
    
}