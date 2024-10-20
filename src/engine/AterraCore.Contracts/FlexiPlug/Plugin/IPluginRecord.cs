﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.ConfigFiles;
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Boot.Logic.PluginLoading.Dto;

namespace AterraCore.Contracts.FlexiPlug.Plugin;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IPluginRecord {
    public PluginId PluginId { get; }
    public IEnumerable<Type> Types { get; }
    public IEnumerable<AssetTypeRecord> AssetTypes { get; }

    public IPluginBootDto PluginBootDto { get; }
    public PluginConfigXml PluginConfigXml { get; }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void InvalidateCaches();
}
