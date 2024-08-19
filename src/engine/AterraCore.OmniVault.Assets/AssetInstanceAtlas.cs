﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.OmniVault.Assets;
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
    private readonly ConcurrentDictionary<Type, List<Ulid>> _assetsByTypes = new();
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
            && _assetInstances.TryGetValue(existingUlid, out IAssetInstance? existingInstance)
            && existingInstance is T convertedInstance) {
            instance = convertedInstance;
            return true;
        }
        
        if (!factory.TryCreate(registration, out instance)) return false;
        instance.AssetId = registration.AssetId;
        instance.InstanceId = predefinedUlid ?? Ulid.NewUlid();

        if (_assetInstances.TryAdd(instance.InstanceId, instance)) {
            Ulid ulid = instance.InstanceId;
            _assetsByTypes.AddOrUpdate(
                typeof(T),
                _ => [ulid],
                (_, list) => { list.Add(ulid); return list; });
            
            if (registration.IsSingleton) {
                _singletonAssetInstances.TryAdd(instance.AssetId, instance.InstanceId);
            }
            return true;
        }
        logger.Debug("{Ulid} could not be added to atlas", instance.InstanceId);
        return false;
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
        if (!_assetInstances.TryGetValue(instanceId, out IAssetInstance? assetInstance)) return false;
        if (assetInstance is not T convertedInstance) return false;
        instance = convertedInstance;
        return true;
    }

    public bool TryGetSingleton<T>(AssetId assetId, [NotNullWhen(true)] out T? instance) where T : class, IAssetInstance {
        instance = default;
        if (!_singletonAssetInstances.TryGetValue(assetId, out Ulid ulid)) return false;
        if (!_assetInstances.TryGetValue(ulid, out IAssetInstance? assetInstance)) return false;
        if (assetInstance is not T convertedInstance) return false;
        instance = convertedInstance;
        return true;
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
        if (!TryGetOrCreate(type, ulid, out T? instance)) throw new ArgumentException($"Asset Id {ulid} not found");
        return instance;
    }

    public T GetOrCreate<T>(AssetId assetId, Ulid? ulid = null) where T : class, IAssetInstance {
        if (!TryGetOrCreate(assetId, ulid, out T? instance)) throw new ArgumentException($"Asset Id {ulid} not found");
        return instance;
    }

    public IEnumerable<T> OfType<T>() where T : class, IAssetInstance {
        if (!_assetsByTypes.TryGetValue(typeof(T), out List<Ulid>? assetIds)) yield break;
        foreach (Ulid assetId in assetIds) {
            if (TryGet(assetId, out T? instance)) yield return instance;
        }
    }
}