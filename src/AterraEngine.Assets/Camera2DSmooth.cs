// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;
using AterraEngine.Components;
using AterraEngine.OldContracts.Assets;
using AterraEngine.OldContracts.Components;
using AterraEngine.Types;
using Raylib_cs;

namespace AterraEngine.Assets;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class Camera2DSmooth : Asset, ICamera2D {
    private Camera2D? _camera2DCache;
    
    // -----------------------------------------------------------------------------------------------------------------
    // Constructor
    // -----------------------------------------------------------------------------------------------------------------
    public Camera2DSmooth(EngineAssetId id, string? internalName = null) : base(id, internalName) {
        TryAddComponent<ICamera2DComponent, Camera2DSmoothComponent>();
    }

    public Camera2D GetRayLibCamera() {
        if (_camera2DCache is not null) return (Camera2D)_camera2DCache;
        if (!TryGetComponent<ICamera2DComponent>(out var camera2DComponent)) throw new Exception("Camera not defined");
        _camera2DCache = camera2DComponent.Camera;
        return (Camera2D)_camera2DCache;
    }
    
}