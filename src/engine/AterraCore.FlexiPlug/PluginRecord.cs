﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.FlexiPlug.Plugin;
using AterraCore.Contracts.Nexities.Data.Assets;
using AterraCore.Contracts.Nexities.Data.Attributes;
using CodeOfChaos.Extensions;
using System.Reflection;

namespace AterraCore.FlexiPlug;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class PluginRecord : IPluginRecord {
    public PluginId PluginId { get; init; }
    public IEnumerable<Type> Types { get; init; } = [];// DON'T invalidate this !!!

    private Dictionary<Type, AssetTypeRecord>? _assetTypeRecords;
    public IEnumerable<AssetTypeRecord> AssetTypes => (
        _assetTypeRecords ??= Types
            .Where(t =>
                typeof(IAssetInstance).IsAssignableFrom(t)
                && t is { IsInterface: false, IsAbstract: false }
            )
            .Select(t => new { Type = t, AssetAttibute = t.GetCustomAttribute<IAssetAttribute>(false) })
            .Where(box => box.AssetAttibute != null)
            .Select(box => new AssetTypeRecord(
                box.Type,
                box.AssetAttibute!,// We check in the where LINQ
                box.Type.GetCustomAttributes<IOverridesAssetIdAttribute>(),
                box.Type.GetCustomAttributes<IAssetTagAttribute>()
            ))
            .ToDictionary(keySelector: record => record.Type, elementSelector: record => record)
        ).Values;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void InvalidateCaches() {
        _assetTypeRecords = null;
    }
}
