// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Common.Types.FlexiPlug;
using System.Text.RegularExpressions;

namespace AterraCore.Common.Types.Nexities;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public readonly partial struct AssetId : IComparable<AssetId>, IEqualityComparer<AssetId> {
    private static readonly Regex Regex = MyRegex();

    public PluginId PluginId { get; }
    public PartialAssetId Id { get; }

    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    public AssetId(PluginId pluginId, PartialAssetId partialId) {
        PluginId = pluginId;
        Id = partialId;
    }

    public AssetId(string pluginId, PartialAssetId partialId) {
        PluginId = new PluginId(pluginId);
        Id = partialId;
    }

    public AssetId(PluginId pluginId, string partialId) {
        PluginId = pluginId;
        Id = new PartialAssetId(partialId);
    }

    public AssetId(string pluginId, string partialId) {
        PluginId = new PluginId(pluginId);
        Id = new PartialAssetId(partialId);
    }

    public AssetId(string fullId) {
        if (string.IsNullOrWhiteSpace(fullId)) {
            throw new ArgumentException($"Invalid format: {fullId}", nameof(fullId));
        }

        Match match = Regex.Match(fullId
            .Replace("-", "")
            .PadLeft(12, '0')
        );
        if (!match.Success) {
            throw new ArgumentException("Invalid input format.", fullId);
        }

        // because of the internal tag, we can immediately cast to an Uint here, and bypass another regex check
        PluginId = new PluginId(PluginId.CastToUshort(match.Groups[1].Value));
        Id = new PartialAssetId(PartialAssetId.CastToUint(match.Groups[2].Value));
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override string ToString() => $"{PluginId.ToString()}{Id.ToString()}";
    public string ToStringReadable() => $"{PluginId.ToStringReadable()}-{Id.ToStringReadable()}";

    [GeneratedRegex("^([0-9a-fA-F]{4})?([0-9a-fA-F]{8})$")]
    private static partial Regex MyRegex();

    // -----------------------------------------------------------------------------------------------------------------
    // Comparisons
    // -----------------------------------------------------------------------------------------------------------------
    public static bool operator ==(AssetId left, AssetId right) => left.Equals(right);
    public static bool operator !=(AssetId left, AssetId right) => !left.Equals(right);

    public int CompareTo(AssetId other) {
        int pluginIdComparison = PluginId.CompareTo(other.PluginId);
        return pluginIdComparison != 0
            ? pluginIdComparison
            : Id.CompareTo(other.Id);
    }

    public override bool Equals(object? obj) => obj is AssetId other && Equals(other);
    public bool Equals(AssetId x, AssetId y) => x.PluginId.Equals(y.PluginId) && x.Id == y.Id;
    public bool Equals(AssetId other) => PluginId.Equals(other.PluginId) && Id == other.Id;

    public int GetHashCode(AssetId obj) => HashCode.Combine(obj.PluginId, obj.Id);
    public override int GetHashCode() => HashCode.Combine(PluginId, Id);

}
