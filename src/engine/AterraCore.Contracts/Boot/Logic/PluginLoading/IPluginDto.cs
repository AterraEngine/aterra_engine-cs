﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.FlexiPlug;
using AterraCore.Common.Types.Nexities;

namespace AterraCore.Contracts.Boot.Logic.PluginLoading;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IPluginDto {
    PluginValidity Validity { get; }
    IEnumerable<Type> Types { get; }
    PluginId PluginId { get; }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    void SetInvalid();
    IEnumerable<(Type Type, T Attribute)> GetOfAttribute<T>() where T : Attribute;
}