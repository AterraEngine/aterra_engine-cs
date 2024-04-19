// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Contracts.Nexities;
using AterraCore.Types;

namespace AterraCore.Nexities.Assets;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

[AttributeUsage(AttributeTargets.Class)]
public class AssetAttribute(
    string partialId,
    AssetType type = AssetType.Actor,
    AssetInstanceType instanceType = AssetInstanceType.Transient,
    
    params Type[] startingComponents
    
    ) : Attribute {
    
    public PartialAssetId PartialAssetId { get; } = new(partialId); 
    public AssetType Type { get; } = type;
    public AssetInstanceType InstanceType { get; } = instanceType;

    public Type[] StartingComponents = startingComponents.Select(
        t => {
            if (!typeof(IComponent).IsAssignableFrom(t))
                throw new ArgumentException(t + " does not implement IComponent");
            return t;
        }
    ).ToArray();
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public AssetRecord ToRecord(PluginId pluginId, Type attachedObject) {
        return new AssetRecord {
            AssetId = new AssetId(pluginId, PartialAssetId),
            Type = attachedObject, 
            InstanceType = InstanceType, 
            StartingComponentTypes = StartingComponents
        };
    }
}
