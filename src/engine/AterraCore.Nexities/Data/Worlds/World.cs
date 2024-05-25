// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

namespace AterraCore.Nexities.Data.Worlds;

using Contracts.Nexities.Data.Levels;
using Contracts.Nexities.Data.Worlds;

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