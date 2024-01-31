// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Contracts.Assets;
using AterraEngine.Contracts.Components;
using AterraEngine.Contracts.ECS;
using AterraEngine.Contracts.ECS.Camera;
using AterraEngine.Contracts.ECS.Logic;
using AterraEngine.Contracts.ECS.Render;
using AterraEngine.Contracts.ECS.Ui;
using AterraEngine.Core;
using AterraEngine.Core.ECS.Camera;
using AterraEngine.Core.ECS.Logic;
using AterraEngine.Core.ECS.Render;
using AterraEngine.Core.ECS.Ui;
using AterraEngine.Core.Types;
using AterraEngine.Lib.ComponentSystems.Logic;
using AterraEngine.Lib.ComponentSystems.Render;
using Raylib_cs;

namespace AterraEngine.Lib.Actors;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class Level2D(EngineAssetId id, string? internalName) : EngineAsset(id, internalName), ILevel {
    public IAssetNodeRoot Assets { get; set; } = new AssetNodeRoot();
    public ICamera2D Camera2D { get; set; }
    public Color BufferBackground { get; set; }
    
    // -----------------------------------------------------------------------------------------------------------------
    // ECS Systems
    // -----------------------------------------------------------------------------------------------------------------
    public ILogicSystemManager LogicSystems {get;set;} = new LogicSystemManager {
        EntitySystems = [
            EngineServices.CreateWithServices<PlayerInput2DSystem>(),
            EngineServices.CreateWithServices<Transform2DSystem>()
        ]
    };
    
    public IRenderSystemManager  RenderSystems {get;set;} = new RenderSystemManager {
        EntitySystems = [
            EngineServices.CreateWithServices<Render2DSystem>()
        ]
    };
    
    public IUiSystemManager  UiSystems {get;set;} = new UiSystemManager {
        EntitySystems = []
    };

    public ICameraSystemManager CameraSystem { get; set; } = new CameraSystemManager {
        EntitySystems = [
            EngineServices.CreateWithServices<Camera2DSystem>()
        ]
    };
    
    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    private IEnumerable<IActor> GetActors() {
        return Assets.OfType<IActor>();
    }
    
}