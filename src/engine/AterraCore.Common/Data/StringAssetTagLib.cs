// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraCore.Common.Data;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class StringAssetTagLib {
    public static class AterraLib {
        private const string ThisSection = nameof(AterraLib);
        public const string PlayerInputTickData = $"#{ThisSection}:{nameof(PlayerInputTickData)}";
        public const string RenderableData = $"#{ThisSection}:{nameof(RenderableData)}";
    }
}
