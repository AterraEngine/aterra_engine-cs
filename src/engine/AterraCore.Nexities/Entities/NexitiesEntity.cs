﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Nexities.Data.Components;
using AterraCore.Contracts.Nexities.Data.Entities;
using AterraCore.Nexities.Assets;
using System.Collections.Concurrent;

namespace AterraCore.Nexities.Entities;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class NexitiesEntity(params IComponent[] components) : AssetInstance, INexitiesEntity {
    protected readonly ConcurrentDictionary<AssetId, IComponent> ComponentsCache = CreateFromComponents(components);
    public IEnumerable<IComponent> ComponentsArray => ComponentsCache.Values;


    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    private static ConcurrentDictionary<AssetId, IComponent> CreateFromComponents(IEnumerable<IComponent> components) {
        return new ConcurrentDictionary<AssetId, IComponent>(
        components
            .ToDictionary(keySelector: component => component.AssetId, elementSelector: component => component)
            .AsEnumerable());
    }
}