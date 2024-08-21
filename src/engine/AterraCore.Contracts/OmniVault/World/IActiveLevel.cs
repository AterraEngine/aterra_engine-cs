// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Nexities.Components;
using AterraCore.Contracts.Nexities.Levels;
using AterraCore.Contracts.Nexities.Systems;
using AterraCore.Contracts.OmniVault.World.EntityTree;
using Raylib_cs;

namespace AterraCore.Contracts.OmniVault.World;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IActiveLevel {
    public INexitiesLevel2D RawLevelData { get;}
    public IReadOnlyCollection<INexitiesSystem> LogicSystems { get; }
    public IReadOnlyCollection<INexitiesSystem> RenderSystems { get; }
    public IEntityNodeTree ActiveEntityTree { get; }
    
    public IRaylibCamera2D? Camera2DEntity { get; }
    public IEnumerable<AssetId> TextureAssetIds { get; }
}
