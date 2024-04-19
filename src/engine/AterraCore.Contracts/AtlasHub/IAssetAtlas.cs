﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using AterraCore.Types;

namespace AterraCore.Contracts.AtlasHub;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public interface IAssetAtlas {
    public IReadOnlyDictionary<AssetId, AssetRecord> Dictionary { get; }
    
    public bool TryRegisterAsset<T>(PluginId pluginId, PartialAssetId partialAssetId, [NotNullWhen(true)] out AssetId? registeredId);
    public bool TryGetAssetRecord(AssetId assetId, [NotNullWhen(true)] out AssetRecord? type);
    public bool TryGetAssetType(string assetId, [NotNullWhen(true)] out AssetRecord? type);
}