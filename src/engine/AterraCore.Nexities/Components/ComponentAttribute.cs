// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Nexities.Components;
using AterraCore.Nexities.Assets;
using AterraCore.Types;

namespace AterraCore.Nexities.Components;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[AttributeUsage(AttributeTargets.Class)]
public class ComponentAttribute(
    string? partialId, 
    AssetInstanceType instanceType = AssetInstanceType.Multiple
    
    ) : AssetAttribute(partialId, AssetType.Component, instanceType);