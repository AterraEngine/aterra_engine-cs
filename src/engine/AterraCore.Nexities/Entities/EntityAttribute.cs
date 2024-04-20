// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Nexities.Assets;
using AterraCore.Types;

namespace AterraCore.Nexities.Entities;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class EntityAttribute(
    string partialId, 
    AssetInstanceType instanceType = AssetInstanceType.Multiple
    
    ) : AssetAttribute(partialId, AssetType.Entity, instanceType);