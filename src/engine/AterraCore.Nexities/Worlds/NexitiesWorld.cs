// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Nexities.Data.Levels;
using AterraCore.Contracts.Nexities.Data.Worlds;
using JetBrains.Annotations;

namespace AterraCore.Nexities.Worlds;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class NexitiesWorld : INexitiesWorld {
    public INexitiesLevel? LoadedLevel { get; private set; }

    public bool TryLoadLevel(INexitiesLevel level) {
        LoadedLevel = level;
        return true;
    }
}
