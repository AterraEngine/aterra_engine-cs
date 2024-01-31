﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Contracts.ECS;
namespace AterraEngine.Core.ECS;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class EntitySystem<T> : IEntitySystem<T>{
    public abstract Type[] ComponentTypes { get; }
    public T ConvertEntity(IEntity entity) => (T)entity;
}