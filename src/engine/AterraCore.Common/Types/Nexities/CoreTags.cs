﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

namespace AterraCore.Common.Types.Nexities;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Flags]
public enum CoreTags : ulong {
    // NO UNDEFINED
    Asset = 1 << 00,
    Component = 1 << 01,
    Entity = 1 << 02,
    System = 1 << 03,

    RenderSystem = 1 << 04,
    LogicSystem = 1 << 05
}