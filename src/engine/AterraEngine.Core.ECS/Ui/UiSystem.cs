// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;
using AterraEngine.Contracts.ECS;
using AterraEngine.Contracts.ECS.Ui;
namespace AterraEngine.Core.ECS.Ui;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class UiSystem<T> : EntitySystem<T>, IUiSystem<T> where T : IEntity{
    public abstract void Process(T entity, float deltaTime, Vector2 worldToScreen);
}