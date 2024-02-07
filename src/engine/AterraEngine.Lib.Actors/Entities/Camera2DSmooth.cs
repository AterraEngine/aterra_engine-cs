// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Contracts.Assets;
using AterraEngine.Contracts.Components;
using AterraEngine.Core.Types;
using AterraEngine.Lib.Components;
using Raylib_cs;
namespace AterraEngine.Lib.Actors.Entities;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class Camera2DSmooth : Asset, ICamera2D {
    private Camera2D? _camera2DCache;
    private ICamera2DComponent? _camera2DComponent { get; set; }
    public ICamera2DComponent Camera2DComponent => _camera2DComponent ??= GetComponent<ICamera2DComponent>();
    
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