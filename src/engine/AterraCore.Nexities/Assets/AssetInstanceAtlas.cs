// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Nexities.Data.Assets;
using AterraCore.DI;
using AterraCore.Nexities.Attributes;
using CodeOfChaos.Extensions;
using JetBrains.Annotations;
using Serilog;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace AterraCore.Nexities.Assets;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class AssetInstanceAtlas(ILogger logger, IAssetAtlas assetAtlas) : IAssetInstanceAtlas {
    private readonly ConcurrentDictionary<Guid, IAssetInstance> _assetInstances = new();
    private readonly ConcurrentDictionary<Type, ConcurrentBag<Guid>> _assetsByTypes = new();
    public int TotalCount => _assetInstances.Count;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public bool TryCreate<T>(AssetId assetId, [NotNullWhen(true)] out T? instance, Guid? predefinedGuid = null) where T : class, IAssetInstance {
        instance = default;
        if (!assetAtlas.TryGetRegistration(assetId, out AssetRegistration registration)) {
            logger.Warning("Asset Id {id} could not be matched to a Type", assetId);
            return false;
        }

        // Will work for Entities which have DI injected components,
        //      because all components have their interface mapped to their assetId
        // Will work for others as well
        ConstructorInfo constructor = registration.Type.GetConstructors().First();
        object[] parameters = constructor.GetParameters()
            .Select(p => {
                if (!assetAtlas.TryGetAssetId(p.ParameterType, out AssetId paramAssetId)) {
                    return !EngineServices.TryGetService(p.ParameterType, out object? output)
                        ? EngineServices.CreateWithServices<object>(p.ParameterType)
                        : output;
                }

                if (p.GetCustomAttribute<InjectAsAttribute>() is not {} refersToAttribute)
                    return EngineServices.CreateNexitiesAsset<IAssetInstance>(
                    !assetAtlas.TryGetType(paramAssetId, out Type? classType)
                        ? p.ParameterType
                        : classType
                    );

                logger.Debug("Parameter type {t} refers to instance {guid} ", p.ParameterType, refersToAttribute.Guid);

                if (TryGetOrCreate(paramAssetId, refersToAttribute.Guid, out IAssetInstance? instance)) {
                    logger.Debug("Found or created an instance");
                    return instance;
                }
                logger.Warning("No instance could be created");
                return EngineServices.CreateWithServices<object>(p.ParameterType);

            })
            .ToArray();

        // ReSharper disable once CoVariantArrayConversion
        instance = (T)constructor.Invoke(parameters);
        instance.AssetId = registration.AssetId;

        // Update the generated
        instance.Guid = predefinedGuid ?? Guid.NewGuid();

        // Finally add the instance
        if (_assetInstances.TryAdd(instance.Guid, instance)) {
            if (!_assetsByTypes.TryAddToBagOrCreateBag(typeof(T), instance.Guid)) {
                logger.Debug("{guid} could not be added to Type bag of {T}", instance.Guid, typeof(T));
            }
            logger.Debug("{guid} added to atlas", instance.Guid);
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

    public bool TryGetOrCreate<T>(Type type, Guid? guid, [NotNullWhen(true)] out T? instance) where T : class, IAssetInstance =>
        guid is not null && TryGet((Guid)guid, out instance)
        || TryCreate(type, out instance, predefinedGuid: guid);

    public bool TryGetOrCreate<T>(AssetId assetId, Guid? guid, [NotNullWhen(true)] out T? instance) where T : class, IAssetInstance =>
        guid is not null && TryGet((Guid)guid, out instance)
        || TryCreate(assetId, out instance, predefinedGuid: guid);

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