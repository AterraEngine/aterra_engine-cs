// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Attributes.DI;
using AterraCore.Common.Data;
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.OmniVault.Assets;
using AterraCore.Contracts.PoolCorps;
using JetBrains.Annotations;
using Serilog;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace AterraCore.AssetVault;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
[Singleton<IAssetInstanceAtlas>]
public class AssetInstanceAtlas(ILogger logger, IAssetAtlas assetAtlas, IAssetInstanceFactory factory, IUlidPools ulidPools) : IAssetInstanceAtlas {
    private readonly ConcurrentDictionary<Ulid, IAssetInstance> _assetInstances = new();
    private readonly ConcurrentDictionary<Type, Lazy<ConcurrentDictionary<Ulid, byte>>> _assetsByTypes = new();
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
            && _singletonAssetInstances.TryGetValue(registration.AssetId, out Ulid existingUlid)) {
            return TryGet(existingUlid, out instance);
        }

        if (!factory.TryCreate(registration, predefinedUlid ?? Ulid.NewUlid(), out instance)) return false;

        // Add or update directly
        T assetInstance = instance;
        if (!_assetInstances.TryAdd(assetInstance.InstanceId, assetInstance)) {
            if (!_assetInstances.TryUpdate(assetInstance.InstanceId, assetInstance, assetInstance)) {
                return false;
            }
        }

        foreach (Type? implementedType in registration.DerivedInterfaceTypes.Concat([registration.Type])) {
            Lazy<ConcurrentDictionary<Ulid, byte>> lazyBag = _assetsByTypes.GetOrAdd(implementedType, valueFactory: _ => new Lazy<ConcurrentDictionary<Ulid, byte>>(() => new ConcurrentDictionary<Ulid, byte>()));
            lazyBag.Value.TryAdd(instance.InstanceId, 0);
        }

        if (registration.IsSingleton)
            _singletonAssetInstances.TryAdd(instance.AssetId, instance.InstanceId);

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

    public bool TryGetOrCreate<T>(Type type, [NotNullWhen(true)] out T? instance, Ulid? ulid = null) where T : class, IAssetInstance {
        instance = null;
        return ulid is {} ulidCasted && TryGet(ulidCasted, out instance)
            || assetAtlas.TryGetAssetId(type, out AssetId assetId)
            && TryCreate(assetId, out instance, ulid);
    }

    public bool TryGetOrCreate<T>(AssetId assetId, [NotNullWhen(true)] out T? instance, Ulid? ulid = null) where T : class, IAssetInstance =>
        ulid is not null && TryGet((Ulid)ulid, out instance)
        || TryCreate(assetId, out instance, ulid);

    public bool TryGetOrCreate<T>(AssetId assetId, [NotNullWhen(true)] out T? instance, Action<T> afterCreation, Ulid? ulid = null) where T : class, IAssetInstance {
        if ((ulid is not {} ulidCasted || !TryGet(ulidCasted, out instance))
            && !TryCreate(assetId, out instance, ulid)) return false;

        afterCreation(instance);
        return true;
    }

    public bool TryGetOrCreateSingleton<T>(AssetId assetId, [NotNullWhen(true)] out T? instance, Ulid? ulid = null) where T : class, IAssetInstance =>
        TryGetSingleton(assetId, out instance)
        || TryCreate(assetId, out instance, ulid);

    public bool TryGetOrCreateSingleton<T>(AssetId assetId, [NotNullWhen(true)] out T? instance, Action<T> afterCreation, Ulid? ulid = null) where T : class, IAssetInstance {
        if (TryGetSingleton(assetId, out instance)) return true;
        if (!TryCreate(assetId, out instance, ulid)) return false;

        afterCreation(instance);
        return true;
    }

    public T GetOrCreate<T>(Type type, Ulid? ulid = null) where T : class, IAssetInstance {
        if (TryGetOrCreate(type, out T? instance, ulid)) return instance;

        logger.Error("Asset instance could not be created from {t} as {ulid}", type, ulid);
        throw new ArgumentException($"Asset instance could not be created from {type} as {ulid}");
    }

    public T GetOrCreate<T>(AssetId assetId, Ulid? ulid = null) where T : class, IAssetInstance {
        if (!TryGetOrCreate(assetId, out T? instance, ulid)) throw new ArgumentException($"Asset Id {ulid} not found");

        return instance;
    }

    public IEnumerable<T> OfType<T>() where T : class, IAssetInstance {
        HashSet<Ulid> alreadyYielded = ulidPools.UlidHashSetPool.Get();
        if (!_assetsByTypes.TryGetValue(typeof(T), out Lazy<ConcurrentDictionary<Ulid, byte>>? assetIds)) yield break;

        foreach (Ulid assetId in assetIds.Value.Keys) {
            if (TryGet(assetId, out T? instance) && alreadyYielded.Add(instance.InstanceId)) {
                yield return instance;
            }
        }

        ulidPools.UlidHashSetPool.Return(alreadyYielded);
    }
    public IEnumerable<T> OfTag<T>(CoreTags tags) where T : class, IAssetInstance {
        HashSet<Ulid> alreadyYielded = ulidPools.UlidHashSetPool.Get();
        if (!_assetsByTypes.TryGetValue(typeof(T), out Lazy<ConcurrentDictionary<Ulid, byte>>? instanceIds)) yield break;

        foreach (Ulid id in instanceIds.Value.Keys) {
            if (_assetInstances.TryGetValue(id, out IAssetInstance? instance)
                && instance is T convertedInstance
                && assetAtlas.TryGetRegistration(instance.AssetId, out AssetRegistration registration)
                && registration.CoreTags.HasFlag(tags)
                && alreadyYielded.Add(id)) {
                yield return convertedInstance;
            }
        }

        ulidPools.UlidHashSetPool.Return(alreadyYielded);

    }
    public IEnumerable<T> OfAssetId<T>(AssetId assetId) where T : class, IAssetInstance {
        if (!assetAtlas.TryGetAssetType(assetId, out Type? type)) {
            logger.Debug("No type found");
            yield break;
        }

        if (!_assetsByTypes.TryGetValue(type, out Lazy<ConcurrentDictionary<Ulid, byte>>? assetIds)) {
            logger.Debug("No Previously created found");
            yield break;
        }

        foreach (Ulid instanceId in assetIds.Value.Keys) {
            logger.Debug(" {u}", instanceId);
            if (!TryGet(instanceId, out T? instance)) continue;

            logger.Debug("Found the instance");
            yield return instance;
        }
    }
}
