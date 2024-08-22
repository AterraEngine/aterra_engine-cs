﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.OmniVault.Assets;
using CodeOfChaos.Extensions.Serilog;
using JetBrains.Annotations;
using Serilog;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace AterraCore.OmniVault.Assets;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class AssetInstanceAtlas(ILogger logger, IAssetAtlas assetAtlas, IAssetInstanceFactory factory) : IAssetInstanceAtlas {
    private readonly ConcurrentDictionary<Ulid, IAssetInstance> _assetInstances = new();
    private readonly ConcurrentDictionary<Type, ConcurrentDictionary<Ulid, byte>> _assetsByTypes = new();
    private readonly ConcurrentDictionary<AssetId, Ulid> _singletonAssetInstances = new();
    
    public int TotalCount => _assetInstances.Count;
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public bool TryCreate<T>(AssetId assetId, [NotNullWhen(true)] out T? instance, Ulid? predefinedUlid = null) where T : class, IAssetInstance {
        instance = default;
        if (!assetAtlas.TryGetRegistration(assetId, out AssetRegistration registration)) {
            logger.Warning("Asset Id {id} could not be matched to a registration", assetId);
            return false;
        }

        // check if it is a singleton and already exists
        if (registration.IsSingleton
            && _singletonAssetInstances.TryGetValue(registration.AssetId, out Ulid existingUlid)
            && TryGet(existingUlid, out instance)) {
            return true;
        }

        Ulid ulid = predefinedUlid ?? Ulid.NewUlid();
        if (!factory.TryCreate(registration, ulid, out instance)) return false;
        T assetInstance = instance;
        
        // Add or update directly
        _assetInstances[assetInstance.InstanceId] = assetInstance;
        
        foreach (Type implementedType in registration.DerivedInterfaceTypes) {
            _assetsByTypes
                .GetOrAdd(implementedType, _ => new ConcurrentDictionary<Ulid, byte>())
                .TryAdd(assetInstance.InstanceId, 0);
        }
            
        if (registration.IsSingleton) 
            _singletonAssetInstances.TryAdd(assetInstance.AssetId, assetInstance.InstanceId);
        
        return true;
    }

    public bool TryCreate<T>([NotNullWhen(true)] out T? instance, Ulid? predefinedUlid = null) where T : class, IAssetInstance =>
        TryCreate(typeof(T), out instance, predefinedUlid);

    public bool TryCreate<T>(Type type, [NotNullWhen(true)] out T? instance, Ulid? predefinedUlid = null) where T : class, IAssetInstance {
        instance = null;
        return assetAtlas.TryGetAssetId(type, out AssetId assetId)
               && TryCreate(assetId, out instance, predefinedUlid);
    }

    public bool TryGet<T>(Ulid instanceId, [NotNullWhen(true)] out T? instance) where T : class, IAssetInstance {
        instance = default;
        if (!_assetInstances.TryGetValue(instanceId, out IAssetInstance? assetInstance) 
            || assetInstance is not T convertedInstance) return false;
        instance = convertedInstance;
        return true;
    }

    public bool TryGetSingleton<T>(AssetId assetId, [NotNullWhen(true)] out T? instance) where T : class, IAssetInstance {
        instance = default;
        return _singletonAssetInstances.TryGetValue(assetId, out Ulid ulid) && TryGet(ulid, out instance);
    }

    public bool TryGetOrCreate<T>(Type type, Ulid? ulid, [NotNullWhen(true)] out T? instance) where T : class, IAssetInstance =>
        ulid is not null && TryGet((Ulid)ulid, out instance)
        || TryCreate(type, out instance, predefinedUlid: ulid);

    public bool TryGetOrCreate<T>(AssetId assetId, Ulid? ulid, [NotNullWhen(true)] out T? instance) where T : class, IAssetInstance =>
        ulid is not null && TryGet((Ulid)ulid, out instance)
        || TryCreate(assetId, out instance, predefinedUlid: ulid);

    public bool TryGetOrCreateSingleton<T>(AssetId assetId, [NotNullWhen(true)] out T? instance) where T : class, IAssetInstance =>
        TryGetSingleton(assetId, out instance)
        || TryCreate(assetId, out instance);

    public T GetOrCreate<T>(Type type, Ulid? ulid = null) where T : class, IAssetInstance {
        if (TryGetOrCreate(type, ulid, out T? instance)) return instance;
        
        logger.Error("Asset instance could not be created from {t} as {ulid}", type, ulid);
        throw new ArgumentException($"Asset instance could not be created from {type} as {ulid}");
    }

    public T GetOrCreate<T>(AssetId assetId, Ulid? ulid = null) where T : class, IAssetInstance {
        if (!TryGetOrCreate(assetId, ulid, out T? instance)) throw new ArgumentException($"Asset Id {ulid} not found");
        return instance;
    }

    public IEnumerable<T> OfType<T>() where T : class, IAssetInstance {
        HashSet<Ulid> alreadyYielded = [];
        if (!_assetsByTypes.TryGetValue(typeof(T), out ConcurrentDictionary<Ulid, byte>? assetIds)) yield break;
        foreach (Ulid assetId in assetIds.Keys) {
            if (TryGet(assetId, out T? instance) && alreadyYielded.Add(instance.InstanceId)) {
                yield return instance;
            }
        }
    }
    public IEnumerable<T> OfTag<T>(CoreTags tags) where T : class, IAssetInstance {
        var alreadyYielded = new HashSet<Ulid>();
        if (!_assetsByTypes.TryGetValue(typeof(T), out ConcurrentDictionary<Ulid, byte>? instanceIds)) yield break;
        foreach (Ulid id in instanceIds.Keys) {
            if (_assetInstances.TryGetValue(id, out IAssetInstance? instance) 
                && instance is T convertedInstance 
                && assetAtlas.TryGetRegistration(instance.AssetId, out AssetRegistration registration) 
                && registration.CoreTags.HasFlag(tags) 
                && alreadyYielded.Add(id)) {
                yield return convertedInstance;
            }
        }

    }
}