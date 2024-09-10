// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraCore.Common.Data;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Flags]
public enum CoreTags : ulong {
    Asset = 1 << 0,
    Component = 1 << 1,
    Entity = 1 << 2,
    System = 1 << 3,
    RenderThread = 1 << 4,
    LogicThread = 1 << 5,
    Texture = 1 << 6,
    Singleton = 1 << 7,
    Level = 1 << 8
        
}


public static class CoreTagsExtensions {
    private static string[]? _strings;
    public static string[] AllCoreTags() => _strings ??= Enum.GetNames(typeof(CoreTags));
    private static CoreTags[]? _values;
    public static CoreTags[] AllCoreTagValues() => _values ??= Enum.GetValues<CoreTags>();
}