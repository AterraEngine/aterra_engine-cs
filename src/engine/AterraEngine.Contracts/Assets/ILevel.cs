// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Contracts.ECS;
using AterraEngine.Contracts.ECS.Camera;
using AterraEngine.Contracts.ECS.Logic;
using AterraEngine.Contracts.ECS.Render;
using AterraEngine.Contracts.ECS.Ui;
using Raylib_cs;

namespace AterraEngine.Contracts.Assets;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ILevel : IEngineAsset{
    public IAssetNodeRoot Assets { get; set; }
    public ICamera2D Camera2D { get; set; }
    public Color BufferBackground { get; }
    
    public ILogicSystemManager  LogicSystems {get;set;} 
    public IRenderSystemManager RenderSystems {get;set;} 
    public IUiSystemManager     UiSystems {get;set;} 
    public ICameraSystemManager CameraSystem {get;set;} 
}