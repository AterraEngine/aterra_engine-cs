// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.ConfigFiles;
using AterraCore.Common.Data;
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Boot.Logic.PluginLoading.Dto;
using AterraCore.Contracts.FlexiPlug;
using AterraCore.Contracts.FlexiPlug.Plugin;
using CodeOfChaos.Extensions;
using JetBrains.Annotations;
using Serilog;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.IO.Compression;

namespace AterraCore.FlexiPlug;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class PluginAtlas(ILogger logger) : IPluginAtlas {
    private ILogger Logger => logger.ForContext<PluginAtlas>();

    private int? _totalAssetCountCache;
    private LinkedList<IPluginRecord> _plugins = [];
    public IReadOnlyCollection<IPluginRecord> Plugins => [.._plugins];

    private readonly HashSet<PluginId> _pluginIds = [];
    public IReadOnlySet<PluginId> PluginIds => _pluginIds;

    private ImmutableDictionary<string, ZipArchive> _pluginZipArchive = ImmutableDictionary<string, ZipArchive>.Empty;

    public int TotalAssetCount => _totalAssetCountCache ??= _plugins.SelectMany(p => p.AssetTypes).Count();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void ImportLoadedPluginDtos(Span<IPluginBootDto> plugins) {
        foreach (IPluginBootDto plugin in plugins) {
            _plugins.AddLast(new PluginRecord {
                PluginId = plugin.PluginNameSpaceId,
                Types = plugin.Types,
                PluginBootDto = plugin,
                PluginConfigXml = plugin.TryGetPluginConfig(out PluginConfigXml? config)
                    ? config
                    : new PluginConfigXml()
            });
            _pluginIds.Add(plugin.PluginNameSpaceId);
        }
    }
    public void InvalidateAllCaches() {
        foreach (IPluginRecord plugin in _plugins) plugin.InvalidateCaches();
    }

    #region Get Registrations
    public IEnumerable<AssetRegistration> GetAssetRegistrations(PluginId? pluginNameSpace = null, CoreTags? filter = null) {
        // Given this is only done once (during project startup), caching this seems a bit unnecessary.
        return _plugins
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

        IPluginRecord? pluginRecord = _plugins.FirstOrDefault(p => p.PluginId == pluginId);
        if (pluginRecord is not { PluginBootDto.FilePath: var filePath } || !filePath.IsNotNullOrEmpty()) {
            logger.Debug("plugin record did not exist {r}", pluginRecord);
            return false;
        }
        if (!TryGetOrCreateZipArchive(filePath, out ZipArchive? zipArchive)) {
            logger.Debug("zip archive did not exist {r}", filePath);
            return false;
        }
        ZipArchiveEntry? fileEntry = zipArchive.GetEntry(internalFilePath);
        if (fileEntry is null) {
            logger.Debug("Could not attain file Entry {r}", internalFilePath);
            return false;
        }

        using var memoryStream = new MemoryStream();
        using Stream stream = fileEntry.Open();
        stream.CopyTo(memoryStream);
        bytes = memoryStream.ToArray();

        return true;
    }
}
