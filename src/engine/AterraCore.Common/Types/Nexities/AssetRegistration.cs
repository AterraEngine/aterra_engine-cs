// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Reflection;
using System.Runtime.InteropServices;

namespace AterraCore.Common.Types.Nexities;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[StructLayout(LayoutKind.Sequential)]
public record struct AssetRegistration(
    AssetId AssetId,
    Type Type
) {
    public IEnumerable<Type> InterfaceTypes { get; init; } = [];
    public IEnumerable<Type> DerivedInterfaceTypes { get; } = GetAllInterfaces(Type).ToList();

    // Data from IAssetAttribute
    public CoreTags CoreTags { get; init; } = 0;

    // Data from IAssetTagAttribute
    public IEnumerable<string> StringTags { get; init; } = [];

    // Data from AbstractOverwritesAssetTagAttribute
    public IEnumerable<AssetId> OverridableAssetIds { get; init; } = [];

    private ConstructorInfo? _constructor = null;
    public ConstructorInfo Constructor => _constructor ??= Type.GetConstructors().First();
    
    private bool? _isSingleton;
    public bool IsSingleton => _isSingleton ??= CoreTags.HasFlag(CoreTags.Singleton);

    // -----------------------------------------------------------------------------------------------------------------
    // Helper Methods
    // -----------------------------------------------------------------------------------------------------------------
    private static HashSet<Type> GetAllInterfaces(Type type) {
        var interfaces = new HashSet<Type>();
        var typeQueue = new Queue<Type>();
        typeQueue.Enqueue(type);

        while (typeQueue.TryDequeue(out Type? currentType)) {
            foreach (Type @interface in currentType.GetInterfaces())
                if (!IsExcludedNamespace(@interface) && interfaces.Add(@interface)) typeQueue.Enqueue(@interface);
            if (currentType.BaseType != null) typeQueue.Enqueue(currentType.BaseType);
        }
        
        return interfaces;

        // Helper method to check if a type's namespace should be excluded
        bool IsExcludedNamespace(Type t) => t.Namespace != null && t.Namespace.StartsWith("System");
    }
}
