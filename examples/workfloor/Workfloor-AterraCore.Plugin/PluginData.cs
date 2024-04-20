﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Contracts.FlexiPlug;
using AterraCore.Types;

namespace Workfloor_AterraCore.Plugin;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public class PluginData(int id) : IPluginData {
    public PluginId Id { get; } = new(id);
    
    private int _partialAssetIdCounter;
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public int GetNewPartialAssetId() => _partialAssetIdCounter++; // TODO might have to be a little bit smarter than this, but fine for now
}