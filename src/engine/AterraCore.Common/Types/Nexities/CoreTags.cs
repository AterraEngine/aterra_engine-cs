﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraCore.Common.Types.Nexities;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Flags]
public enum CoreTags : ulong {
    Asset        = 1 << 0,
    Component    = 1 << 1,
    Entity       = 1 << 2,
    System       = 1 << 3,
    RenderSystem = 1 << 4,
    LogicSystem  = 1 << 5,
    Texture      = 1 << 6,
    Singleton    = 1 << 7,
    Level        = 1 << 8,
}
