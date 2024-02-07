// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using OldAterraEngine.Contracts.Assets;
using OldAterraEngine.Contracts.ECS;
using OldAterraEngine.Contracts.ECS.EntityCombinations;
using OldAterraEngine.Core.ECS;
using OldAterraEngine.Core;
using OldAterraEngine.Core.Types;
using OldAterraEngine.Lib.ComponentSystems.Logic;
using OldAterraEngine.Lib.ComponentSystems.Render;
using Raylib_cs;

namespace OldAterraEngine.Lib.Actors;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class Level2D : EngineAsset, ILevel {
    public IAssetNodeRoot Assets { get; set; } = new AssetNodeRoot();
    public ICamera2D Camera2D { get; set; }
    public Color BufferBackground { get; set; }

    private ICamera2D? _cachedCamera { get; set; } 
    private IPlayer2DEntity? _cachedPlayer { get; set; } 
    
    // -----------------------------------------------------------------------------------------------------------------
    // ECS Systems
    // -----------------------------------------------------------------------------------------------------------------
    public IEntityComponentSystemManager LogicManager { get; set; } = new EntityComponentSystemManager();
    public IEntityComponentSystemManager RenderManager { get; set; } = new EntityComponentSystemManager();

    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    public Level2D(EngineAssetId id, string? internalName) : base(id, internalName) {
        LogicManager.AddSystem( EngineServices.CreateWithServices<PlayerInput2DSystem>());
        LogicManager.AddSystem( EngineServices.CreateWithServices<Transform2DSystem>());
        LogicManager.AddSystem( EngineServices.CreateWithServices<Camera2DSystem>());
        
        RenderManager.AddSystem(EngineServices.CreateWithServices<Render2DSystem>());
    }
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    private IEnumerable<IActor> GetActors() {
        return Assets.OfType<IActor>();
    }

    public IPlayer2DEntity GetPlayer() => _cachedPlayer ??= Assets.Flat().OfType<IPlayer2DEntity>().First();
    public ICamera2D GetCamera() => _cachedCamera ??= Assets.Flat().OfType<ICamera2D>().First();
}