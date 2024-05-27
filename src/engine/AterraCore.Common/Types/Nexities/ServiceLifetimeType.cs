// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

namespace AterraCore.Common.Types.Nexities;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public enum ServiceLifetimeType {
    /// <summary> Each asset ID points to exactly one shared instance of the asset.</summary>
    Singleton = 1,

    /// <summary> Each asset ID points to individual instances of the asset, creating a new one each time. </summary>
    Multiple = 2,

    /// <summary>
    ///     Each asset ID points to a reusable instance of the asset from a pool of asset instances, avoiding the cost of
    ///     frequently creating and destroying instances.
    /// </summary>
    Pooled = 3
}