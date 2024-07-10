// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Nexities.Data.Levels;
using AterraCore.Contracts.Nexities.Data.Worlds;

namespace AterraCore.Nexities.Worlds;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class NexitiesWorld : INexitiesWorld {
    public INexitiesLevel? LoadedLevel { get; private set; }

    public bool TryLoadLevel(INexitiesLevel level) {
        LoadedLevel = level;
        return true;
    }
}
