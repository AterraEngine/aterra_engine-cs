// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.OmniVault.Assets;
using AterraCore.DI;
using AterraCore.OmniVault.Assets.Attributes;
using CodeOfChaos.Extensions;
using JetBrains.Annotations;
using Serilog;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace AterraCore.OmniVault.Assets;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class AssetInstanceAtlas(ILogger logger, IAssetAtlas assetAtlas) : IAssetInstanceAtlas {
    private readonly ConcurrentDictionary<Guid, IAssetInstance> _assetInstances = new();
    private readonly ConcurrentDictionary<Type, ConcurrentBag<Guid>> _assetsByTypes = new();
    private readonly ConcurrentDictionary<AssetId, Guid> _singletonAssetInstances = new();
    
    public int TotalCount => _assetInstances.Count;

    // -----------------------------------------------------------------------------------------------------------------
    // Helper Methods
    // -----------------------------------------------------------------------------------------------------------------
    private object[] CreateParameters(ref AssetRegistration registration) {
        if (registration.ConstructorParameters is not null) {
            return registration.ConstructorParameters;
        }
        
        object[] parameters = registration.Constructor.GetParameters()
            .Select(p => {
                if (!assetAtlas.TryGetAssetId(p.ParameterType, out AssetId paramAssetId)) {
                    return !EngineServices.TryGetService(p.ParameterType, out object? service)
                        ? EngineServices.CreateWithServices<object>(p.ParameterType)
                        : service;
                }
                if (p.GetCustomAttribute<InjectAsAttribute>() is not {} refersToAttribute) {
                    return EngineServices.CreateNexitiesAsset<IAssetInstance>(
                        !assetAtlas.TryGetType(paramAssetId, out Type? classType)
                            ? p.ParameterType
                            : classType
                    );
                }
                logger.Debug("Parameter type {type} refers to instance {guid}", p.ParameterType, refersToAttribute.Guid);
                if (TryGetOrCreate(paramAssetId, refersToAttribute.Guid, out IAssetInstance? assetInstance)) {
                    logger.Debug("Found or created an instance");
                    return assetInstance;
                }
                logger.Warning("No instance could be created");
                return EngineServices.CreateWithServices<object>(p.ParameterType);
            }).ToArray();
        
        registration.ConstructorParameters = parameters; // Cache the parameters
        
        return parameters;
    }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public bool TryCreate<T>(AssetId assetId, [NotNullWhen(true)] out T? instance, Guid? predefinedGuid = null) where T : class, IAssetInstance {
        instance = default;
        if (!assetAtlas.TryGetRegistration(assetId, out AssetRegistration registration)) {
            logger.Warning("Asset Id {id} could not be matched to a registration", assetId);
            return false;
        }

        // check if it is a singleton and already exists
        if (registration.IsSingleton() 
            && _singletonAssetInstances.TryGetValue(registration.AssetId, out Guid existingGuid)
            && _assetInstances.TryGetValue(existingGuid, out IAssetInstance? existingInstance)
            && existingInstance is T convertedInstance) {
            instance = convertedInstance;
            return true;
        }

        instance = (T)registration.Constructor.Invoke(CreateParameters(ref registration));
        instance.AssetId = registration.AssetId;
        instance.Guid = predefinedGuid ?? Guid.NewGuid();

        if (_assetInstances.TryAdd(instance.Guid, instance)) {
            if (!_assetsByTypes.TryAddToBagOrCreateBag(typeof(T), instance.Guid)) {
                logger.Debug("{guid} could not be added to Type bag of {type}", instance.Guid, typeof(T));
            }
            if (registration.IsSingleton()) {
                _singletonAssetInstances.TryAdd(instance.AssetId, instance.Guid);
            }
            // logger.Debug("{guid} added to atlas", instance.Guid);
            return true;
        }
        logger.Debug("{guid} could not be added to atlas", instance.Guid);
        return false;
    }

    public bool TryCreate<T>([NotNullWhen(true)] out T? instance, Guid? predefinedGuid = null) where T : class, IAssetInstance =>
        TryCreate(typeof(T), out instance, predefinedGuid);

    public bool TryCreate<T>(Type type, [NotNullWhen(true)] out T? instance, Guid? predefinedGuid = null) where T : class, IAssetInstance {
        instance = null;
        return assetAtlas.TryGetAssetId(type, out AssetId assetId)
               && TryCreate(assetId, out instance, predefinedGuid);
    }

    public bool TryGet<T>(Guid instanceId, [NotNullWhen(true)] out T? instance) where T : class, IAssetInstance {
        instance = default;
        if (!_assetInstances.TryGetValue(instanceId, out IAssetInstance? assetInstance)) return false;
        if (assetInstance is not T convertedInstance) return false;
        instance = convertedInstance;
        return true;
    }

    public bool TryGetSingleton<T>(AssetId assetId, [NotNullWhen(true)] out T? instance) where T : class, IAssetInstance {
        instance = default;
        if (!_singletonAssetInstances.TryGetValue(assetId, out Guid guid)) return false;
        if (!_assetInstances.TryGetValue(guid, out IAssetInstance? assetInstance)) return false;
        if (assetInstance is not T convertedInstance) return false;
        instance = convertedInstance;
        return true;
    }

    public bool TryGetOrCreate<T>(Type type, Guid? guid, [NotNullWhen(true)] out T? instance) where T : class, IAssetInstance =>
        guid is not null && TryGet((Guid)guid, out instance)
        || TryCreate(type, out instance, predefinedGuid: guid);

    public bool TryGetOrCreate<T>(AssetId assetId, Guid? guid, [NotNullWhen(true)] out T? instance) where T : class, IAssetInstance =>
        guid is not null && TryGet((Guid)guid, out instance)
        || TryCreate(assetId, out instance, predefinedGuid: guid);

    public bool TryGetOrCreateSingleton<T>(AssetId assetId, [NotNullWhen(true)] out T? instance) where T : class, IAssetInstance =>
        TryGetSingleton(assetId, out instance)
        || TryCreate(assetId, out instance);

    public T GetOrCreate<T>(Type type, Guid? guid = null) where T : class, IAssetInstance {
        if (!TryGetOrCreate(type, guid, out T? instance)) throw new ArgumentException($"Asset Id {guid} not found");
        return instance;
    }

    public T GetOrCreate<T>(AssetId assetId, Guid? guid = null) where T : class, IAssetInstance {
        if (!TryGetOrCreate(assetId, guid, out T? instance)) throw new ArgumentException($"Asset Id {guid} not found");
        return instance;
    }

    public IEnumerable<T> OfType<T>() where T : class, IAssetInstance {
        if (!_assetsByTypes.TryGetValue(typeof(T), out ConcurrentBag<Guid>? assetIds)) yield break;
        foreach (Guid assetId in assetIds) {
            if (TryGet(assetId, out T? instance)) yield return instance;
        }
    }
}