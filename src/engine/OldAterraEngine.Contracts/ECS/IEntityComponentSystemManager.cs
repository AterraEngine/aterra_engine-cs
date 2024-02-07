// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using OldAterraEngine.Contracts.Assets;

namespace OldAterraEngine.Contracts.ECS;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IEntityComponentSystemManager {
    public void AddSystem(IEntityComponentSystem system);
    public void Process(IEnumerable<IAsset?> entities);
}