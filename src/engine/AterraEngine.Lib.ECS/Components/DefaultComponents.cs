// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Common;

namespace AterraEngine.Lib.ECS.Components;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class DefaultComponents {
    internal const uint PTransform2D = 0;
    internal const uint PSprite = 1;
    public static readonly AssetId Transform2D = new(new PluginId(0), new PartialAssetId(PTransform2D) );
    public static readonly AssetId Sprite =      new(new PluginId(0), new PartialAssetId(PSprite));
}