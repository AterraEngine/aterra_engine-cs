// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using OldAterraEngine.Contracts.Assets;
using OldAterraEngine.Contracts.ECS;
namespace OldAterraEngine.Core.ECS;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class EntityComponentSystemManager : IEntityComponentSystemManager {
    public List<object> Systems { get; init; }= [];
    
    public void AddSystem(IEntityComponentSystem system) {
        Systems.Add(system);
    }
    
    public void Process(IEnumerable<IAsset?> entities) {
        foreach (IEntityComponentSystem system in Systems) {
            foreach (IEntity entity in entities) {
                if(system.CheckEntity(entity)) system.Update(entity);
            }
        }
    }
}