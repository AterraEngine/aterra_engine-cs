// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Boot.Logic.PluginLoading.Dto;
using AterraCore.Contracts.FlexiPlug;
using AterraCore.Contracts.FlexiPlug.Plugin;
using CodeOfChaos.Extensions;
using JetBrains.Annotations;
using Serilog;
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
    private LinkedList<IPluginRecord> Plugins { get; } = [];

    private readonly HashSet<PluginId> _pluginIds = [];
    public IReadOnlySet<PluginId> PluginIds => _pluginIds;
    
    public int TotalAssetCount => _totalAssetCountCache ??= Plugins.SelectMany(p => p.AssetTypes).Count();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void ImportLoadedPluginDtos(Span<IPluginBootDto> plugins) {
        foreach (IPluginBootDto plugin in plugins) {
            Plugins.AddLast(new PluginRecord {
                PluginId = plugin.PluginNameSpaceId,
                Types = plugin.Types,
                PluginBootDto = plugin
            });
            _pluginIds.Add(plugin.PluginNameSpaceId);
        }
    }
    public void InvalidateAllCaches() => Plugins.IterateOver(plugin => plugin.InvalidateCaches());

    #region Get Registrations
    public IEnumerable<AssetRegistration> GetAssetRegistrations(PluginId? pluginNameSpace = null, CoreTags? filter = null) {
        // Given this is only done once (during project startup), caching this seems a bit unnecessary.
        return Plugins
            // Filter down to only the plugin we need
            .ConditionalWhere(pluginNameSpace != null, p => p.PluginId == (PluginId)pluginNameSpace!)
            .SelectMany(p => p.AssetTypes
                // Filter down to which Asset Tag we want
                .ConditionalWhere(filter != null, record => record.AssetAttribute.CoreTags.HasFlag(filter!))
                .Select(record => new AssetRegistration(
                    AssetId: record.AssetAttribute.AssetId,
                    Type: record.Type
                ) {
                    InterfaceTypes = record.AssetAttribute.InterfaceTypes,
                    CoreTags = record.AssetAttribute.CoreTags,
                    StringTags = record.AssetTagAttributes.SelectMany(attribute => attribute.Tags),
                    OverridableAssetIds = record.OverwritesAssetIdAttributes.Select(attribute => attribute.AssetId),
                })
            );
    }

    public IEnumerable<AssetRegistration> GetEntityRegistrations(PluginId? pluginNameSpace = null) =>
        GetAssetRegistrations(pluginNameSpace, CoreTags.Entity);

    public IEnumerable<AssetRegistration> GetComponentRegistrations(PluginId? pluginNameSpace = null) =>
        GetAssetRegistrations(pluginNameSpace, CoreTags.Component);
    #endregion

    public bool TryGetFileRawFromPluginZip(
        PluginId pluginId, string internalFilePath, 
        [NotNullWhen(true)] out byte[]? bytes
    ) {
        bytes = null;
        IPluginRecord? pluginRecord = Plugins.FirstOrDefault(p => p.PluginId == pluginId);
        if (pluginRecord is not { PluginBootDto.FilePath: var filePath } || !filePath.IsNotNullOrEmpty()) {
            return false;
        }
        
        ZipArchive archive = ZipFile.OpenRead(filePath);
        ZipArchiveEntry? fileEntry = archive.GetEntry(internalFilePath);
        if (fileEntry is null) return false;
        
        using var memoryStream = new MemoryStream();
        using Stream stream = fileEntry.Open();
        stream.CopyTo(memoryStream);
        bytes = memoryStream.ToArray();
        
        return false;
    }
}
