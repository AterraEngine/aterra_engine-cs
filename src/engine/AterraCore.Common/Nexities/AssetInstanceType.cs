// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

namespace AterraCore.Common.Nexities;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public enum AssetInstanceType {
    /// <summary> Each asset ID points to exactly one shared instance of the asset.</summary>
    Singleton,

    /// <summary> Each asset ID points to individual instances of the asset, creating a new one each time. </summary>
    Multiple,

    /// <summary> Each asset ID points to a reusable instance of the asset from a pool of asset instances, avoiding the cost of frequently creating and destroying instances.  </summary>
    Pooled
}