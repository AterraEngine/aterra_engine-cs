// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraEngine.Core.Types;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public struct EngineAssetId(PluginId pluginId, int value) : IComparable<EngineAssetId>, IEqualityComparer<EngineAssetId> {
    public PluginId PluginId { get; } = pluginId;
    public int Id { get; } = value;

    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------

    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    public override string ToString() {
        return $"{PluginId.ToString().PadLeft(4, '0')}{Id.ToString("X").PadLeft(8, '0')}";
    }

    public int CompareTo(EngineAssetId other) {
        var pluginIdComparison = PluginId.CompareTo(other.PluginId);
        return pluginIdComparison != 0 
            ? pluginIdComparison 
            : Id.CompareTo(other.Id);
    }
    
    public static bool operator ==(EngineAssetId left, EngineAssetId right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(EngineAssetId left, EngineAssetId right)
    {
        return !(left == right);
    }

    public bool Equals(EngineAssetId x, EngineAssetId y) {
        return x.PluginId.Equals(y.PluginId) && x.Id == y.Id;
    }

    public int GetHashCode(EngineAssetId obj) {
        return HashCode.Combine(obj.PluginId, obj.Id);
    }
    
    public bool Equals(EngineAssetId other) {
        return PluginId.Equals(other.PluginId) && Id == other.Id;
    }

    public override bool Equals(object? obj) {
        return obj is EngineAssetId other && Equals(other);
    }

    public override int GetHashCode() {
        return HashCode.Combine(PluginId, Id);
    }

}