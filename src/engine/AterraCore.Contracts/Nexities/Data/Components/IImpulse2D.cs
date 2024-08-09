﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Raylib_cs;
using System.Numerics;

namespace AterraCore.Contracts.Nexities.Data.Components;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IImpulse2D : INexitiesComponent {
    Vector2 TranslationOffset { get; set; }
    Vector2 ScaleOffset { get; set; } 
    float RotationOffset { get; set; }

    void Clear();
}
