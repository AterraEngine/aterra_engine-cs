﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraCore.Common.Data;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public enum ExitCodes {
    GeneralError = 1,
    UnstableFlexiPlugLoadOrder = 2,
    UnstableBootOperationOrder = 3,
    PluginIdsExhausted = 4,
    EngineOverwritten = 5,
    UnableToLoadEngineConfigFile = 6,
}