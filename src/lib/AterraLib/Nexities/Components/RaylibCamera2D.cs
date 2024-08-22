﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraLib.Nexities.Components;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Component<IRaylibCamera2D>(AssetIdLib.AterraCore.Components.RaylibCamera2D)]
[UsedImplicitly]
public class RaylibCamera2D : NexitiesComponent, IRaylibCamera2D {
    public Camera2D Camera {
        get;
        set;
    }
}