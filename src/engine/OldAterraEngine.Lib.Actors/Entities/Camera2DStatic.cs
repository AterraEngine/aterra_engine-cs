// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using OldAterraEngine.Contracts.Assets;
using OldAterraEngine.Contracts.Components;
using OldAterraEngine.Core.Types;
using OldAterraEngine.Lib.Components;
using Raylib_cs;
namespace OldAterraEngine.Lib.Actors.Entities;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class Camera2DStatic : Asset, ICamera2D {
    private Camera2D? _camera2DCache;
    public ICamera2DComponent Camera2DComponent { get; }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Constructor
    // -----------------------------------------------------------------------------------------------------------------
    public Camera2DStatic(EngineAssetId id, string? internalName = null) : base(id, internalName) {
        TryAddComponent<ICamera2DComponent, Camera2DStaticComponent>();
    }

    public Camera2D GetRayLibCamera() {
        if (_camera2DCache is not null) return (Camera2D)_camera2DCache;
        if (!TryGetComponent<ICamera2DComponent>(out ICamera2DComponent? camera2DComponent)) throw new Exception("Camera not defined");
        _camera2DCache = camera2DComponent.Camera;
        return (Camera2D)_camera2DCache;
    }
    
}