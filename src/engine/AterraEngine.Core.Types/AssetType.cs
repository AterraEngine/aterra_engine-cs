// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraEngine.Core.Types;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Flags] // ? Isn't this only useful when an asset can be multiple things?
public enum AssetType {
    Undefined =     0,              // Asset Type was undefined. Not desirable to be used, but better than nothing
    ECSComponent =  1 << 0,         // A component within the ECS Framework
    Actor =         1 << 1,
    
    // StaticActor =   1 << 0,         // Think trees, props, etc... . Actors which remain static throughout the world
    // Texture =       1 << 1,         // Texture file
    // Sprite =        1 << 2,         // Sprite selection from texture file
    // Model =         1 << 3,         // 3d model asset
    // Audio =         1 << 4,         // Audio file
    // Level =         1 << 5,         // Complete scene or level
    // DynamicActor =  1 << 6,         // think animals, npc's, etc... . Actors which have lots of logic to them, move, die, ...
    // Location =      1 << 7,         // Point of location in a level
    // QuestTree =     1 << 8,         // A Tree of quest nodes
    // QuestNode =     1 << 9,         // A node in the QuestTree providing requirements to complete ...
    // Statistic =     1 << 10,        // A static value incremented by events
    // Effect =        1 << 11,        // A value changing over time (usually through f(x) formula)
    // Camera =        1 << 12,        // Cameras
    // Player =        1 << 13,        // Players
    // NPC =           DynamicActor,   // Alias for Dynamic Actors
    // Actor =         StaticActor,    // Alias for Static Actors
    // Font =          1 << 14,        // Font Asset
    // UiLayout =      1 << 15,        // UI layout asset
    // TerrainMatrix = 1 << 16,        // A 3D / 2D Grid of Terrain Tiles
    // TerrainTile =   1 << 17,        // A Tile within the Matrix
    
}