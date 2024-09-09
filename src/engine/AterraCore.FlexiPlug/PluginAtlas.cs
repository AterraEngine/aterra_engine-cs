// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Data;
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.FlexiPlug;
using AterraCore.Contracts.FlexiPlug.Plugin;
using CodeOfChaos.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.IO.Compression;

namespace AterraCore.FlexiPlug;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class PluginAtlas(IServiceProvider provider) : IPluginAtlas {
    private readonly ILogger _logger = provider.GetRequiredService<ILogger>().ForContext<PluginAtlas>();

    public IReadOnlyCollection<IPluginRecord> Plugins { get; init; } = [];
    public FrozenSet<PluginId> PluginIds { get; init; } = new HashSet<PluginId>().ToFrozenSet();
    public ImmutableArray<PluginId> PluginIdsByOrder { get; init; } = [];

    private int? _totalAssetCountCache;
    public int TotalAssetCount => _totalAssetCountCache ??= Plugins.SelectMany(p => p.AssetTypes).Count();
    
    private ImmutableDictionary<string, ZipArchive> _pluginZipArchive = ImmutableDictionary<string, ZipArchive>.Empty;
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    #region Get Registrations
    public IEnumerable<AssetRegistration> GetAssetRegistrations(PluginId? pluginNameSpace = null, CoreTags? filter = null) {
        // Given this is only done once (during project startup), caching this seems a bit unnecessary.
        return Plugins
            // Filter down to only the plugin we need
            .ConditionalWhere(pluginNameSpace != null, predicate: p => p.PluginId == (PluginId)pluginNameSpace!)
            .SelectMany(p => p.AssetTypes
                // Filter down to which Asset Tag we want
                .ConditionalWhere(filter != null, predicate: record => record.AssetAttribute.CoreTags.HasFlag(filter!))
                .Select(record => new AssetRegistration(
                    record.AssetAttribute.AssetId,
                    record.Type
                ) {
                    InterfaceTypes = record.AssetAttribute.InterfaceTypes,
                    CoreTags = record.AssetAttribute.CoreTags,
                    StringTags = record.AssetTagAttributes.SelectMany(attribute => attribute.Tags),
                    OverridableAssetIds = record.OverwritesAssetIdAttributes.Select(attribute => attribute.AssetId)
                })
            );
    }

    public IEnumerable<AssetRegistration> GetEntityRegistrations(PluginId? pluginNameSpace = null) =>
        GetAssetRegistrations(pluginNameSpace, CoreTags.Entity);

    public IEnumerable<AssetRegistration> GetComponentRegistrations(PluginId? pluginNameSpace = null) =>
        GetAssetRegistrations(pluginNameSpace, CoreTags.Component);
    #endregion

    private bool TryGetOrCreateZipArchive(string filePath, [NotNullWhen(true)] out ZipArchive? zipArchive) {
        zipArchive = default;
        // ReSharper disable once SuggestVarOrType_SimpleTypes
        if (_pluginZipArchive.TryGetValue(filePath, out zipArchive)) return true;

        if (!File.Exists(filePath)) return false;
        zipArchive = ZipFile.OpenRead(filePath);
        _pluginZipArchive = _pluginZipArchive.Add(filePath, zipArchive);

        return true;
    }

    public bool TryGetFileRawFromPluginZip(
        PluginId pluginId, string internalFilePath,
        [NotNullWhen(true)] out byte[]? bytes
    ) {
        bytes = default;

        IPluginRecord? pluginRecord = Plugins.FirstOrDefault(p => p.PluginId == pluginId);
        if (pluginRecord is not { PluginBootDto.FilePath: var filePath } || !filePath.IsNotNullOrEmpty()) {
            _logger.Debug("plugin record did not exist {r}", pluginRecord);
            return false;
        }
        
        if (!TryGetOrCreateZipArchive(filePath, out ZipArchive? zipArchive)) {
            _logger.Debug("zip archive did not exist {r}", filePath);
            return false;
        }
        
        ZipArchiveEntry? fileEntry = zipArchive.GetEntry(internalFilePath);
        if (fileEntry is null) {
            _logger.Debug("Could not attain file Entry {r}", internalFilePath);
            return false;
        }

        using var memoryStream = new MemoryStream();
        using Stream stream = fileEntry.Open();
        stream.CopyTo(memoryStream);
        bytes = memoryStream.ToArray();

        return true;
    }

    public bool IsLoadedBefore(PluginId left, PluginId right) {
        int l = PluginIdsByOrder.IndexOf(left);
        int r = PluginIdsByOrder.IndexOf(right);
        return l != -1 && r != -1 && l < r;
    }
    
    public bool IsLoadedAfter(PluginId left, PluginId right) {
        int l = PluginIdsByOrder.IndexOf(left);
        int r = PluginIdsByOrder.IndexOf(right);
        return l != -1 && r != -1 && l > r;
    }
}
