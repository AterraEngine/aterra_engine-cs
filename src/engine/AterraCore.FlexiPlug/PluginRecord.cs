// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Common.Types.FlexiPlug;
using AterraCore.Contracts.FlexiPlug.Plugin;
using AterraCore.Contracts.Nexities.Data.Assets;
using System.Reflection;

namespace AterraCore.FlexiPlug;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public class PluginRecord : IPluginRecord {

    public PluginId Id { get; init; }
    public string ReadableName { get; init; } = "UNDEFINED";

    public IEnumerable<Type> Types { get; init; } = [];// DON'T invalidate this !!!

    private Dictionary<Type, AssetTypeRecord>? _assetTypeRecords;
    public IEnumerable<AssetTypeRecord> AssetTypes => (
        _assetTypeRecords ??= Types
            .Where(t =>
                typeof(IAssetInstance).IsAssignableFrom(t)
                && t is { IsInterface: false, IsAbstract: false }
            )
            .Select(t => new { Type = t, AssetAttibute = t.GetCustomAttribute<AbstractAssetAttribute>(false) })
            .Where(box => box.AssetAttibute != null)
            .Select(box => new AssetTypeRecord(
                box.Type,
                box.AssetAttibute!,// We check in the where LINQ
                box.Type.GetCustomAttributes<AbstractAssetTagAttribute>()
            ))
            .ToDictionary(keySelector: record => record.Type, elementSelector: record => record)
        ).Values;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void InvalidateCaches() {
        _assetTypeRecords = null;
    }
    
    
    // public IEnumerable<ServiceDescriptor> GetNexitiesComponents() {
    //     return Types
    //         .Select(t => new { Type = t, Attribute = t.GetCustomAttribute<ComponentAttribute>(false) })// this way we only get the attribute once
    //         .Where(t => t.Attribute != null)
    //         .Select(t => new ServiceDescriptor(
    //         t.Attribute?.InterfaceType ?? t.Type,
    //         t.Type,
    //         t.Attribute?.ServiceLifetime switch {
    //             ServiceLifetimeType.Singleton => ServiceLifetime.Singleton,
    //             ServiceLifetimeType.Multiple => ServiceLifetime.Transient,
    //             // (AssetInstanceType.Pooled) => ServiceLifetime.Pooled
    //             _ => ServiceLifetime.Transient
    //         }
    //         ));
    // }
    
    // public IEnumerable<ServiceDescriptor> GetNexitiesEntities() {
    //     return Types
    //         .Select(t => new { Type = t, Attribute = t.GetCustomAttribute<EntityAttribute>(false) })// this way we only get the attribute once
    //         .Where(t => t.Attribute != null)
    //         .Select(t => new ServiceDescriptor(
    //         t.Attribute?.Interface ?? t.Type,
    //         t.Type,
    //         t.Attribute?.ServiceLifetime switch {
    //             ServiceLifetimeType.Singleton => ServiceLifetime.Singleton,
    //             ServiceLifetimeType.Multiple => ServiceLifetime.Transient,
    //             _ => ServiceLifetime.Transient
    //         }
    //         ));
    // }
}
