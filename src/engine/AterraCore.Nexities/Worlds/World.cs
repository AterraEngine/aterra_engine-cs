// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Contracts.Nexities.Levels;
using AterraCore.Contracts.Nexities.Worlds;

namespace AterraCore.Nexities.Worlds;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public class World : IWorld {
    public ILevel LoadedLevel { get; private set; }

    public bool TryLoadLevel(ILevel level) {
        LoadedLevel = level;
        return true;
    }
    
}