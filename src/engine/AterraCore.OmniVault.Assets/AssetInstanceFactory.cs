// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Attributes;
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.FlexiPlug;
using AterraCore.Contracts.OmniVault.Assets;
using AterraCore.DI;
using JetBrains.Annotations;
using Serilog;
using System.Buffers;
using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;
using System.Security;

namespace AterraCore.OmniVault.Assets;

// ---------------------------------------------------------------------------------------------------------------------
// Support Code
// ---------------------------------------------------------------------------------------------------------------------
using TActionsArray=Func<object>[];
using TConstructor=Func<object[], object>;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
[Singleton<IAssetInstanceFactory>]
public class AssetInstanceFactory(ILogger logger, IPluginAtlas pluginAtlas) : IAssetInstanceFactory {
    private static readonly ArrayPool<object> ParameterPool = ArrayPool<object>.Shared;
    private readonly FrozenDictionary<AssetId, TActionsArray> _actionsMap = AssembleParameterActions(pluginAtlas);
    private readonly FrozenDictionary<Type, TConstructor> _constructorCache = AssembleConstructorDelegates(pluginAtlas);

    // -----------------------------------------------------------------------------------------------------------------
    //  Methods
    // -----------------------------------------------------------------------------------------------------------------
    public bool TryCreate<T>(AssetRegistration registration, Ulid predefinedUlid, [NotNullWhen(true)] out T? assetInstance) where T : class, IAssetInstance {
        assetInstance = null;
        object[] parameters = ParameterPool.Rent(registration.ConstructorParamLength);

        try {
            if (!_constructorCache.TryGetValue(registration.Type, out TConstructor? constructorDelegate)) return false;
            if (!_actionsMap.TryGetValue(registration.AssetId, out TActionsArray? parameterActions)) return false;

            for (int i = 0; i < parameterActions.Length; i++) parameters[i] = parameterActions[i]();

            if (constructorDelegate(parameters) is not T castedInstance) return false;
            assetInstance = castedInstance;
            assetInstance.AssetId = registration.AssetId;
            assetInstance.InstanceId = predefinedUlid;

            return true;
        }
        catch (Exception e) when (e is MethodAccessException or ArgumentException or TargetInvocationException or TargetParameterCountException or NotSupportedException or SecurityException) {
            logger.Error(e, $"Caught {e.GetType().Name}");
            return false;
        }
        finally {
            ParameterPool.Return(parameters, true);
        }
    }

    public T Create<T>(AssetRegistration registration, Ulid predefinedUlid) where T : class, IAssetInstance {
        TConstructor constructorDelegate = _constructorCache[registration.Type];

        object[] parameters = _actionsMap[registration.AssetId]
            .Select(action => action())
            .ToArray();

        var assetInstance = (T)constructorDelegate(parameters);

        assetInstance.AssetId = registration.AssetId;
        assetInstance.InstanceId = predefinedUlid;

        return assetInstance;
    }

    #region Not a Nexities Asset
    private static object CreateUnknown(ParameterInfo parameter) {
        Type paramType = parameter.ParameterType;
        return paramType.IsInterface && EngineServices.TryGetService(paramType, out object? output)
            ? output
            : EngineServices.CreateWithServices<object>(paramType);
    }
    #endregion

    // -----------------------------------------------------------------------------------------------------------------
    // Constructor Methods
    // -----------------------------------------------------------------------------------------------------------------
    #region Special constructor methods
    private static FrozenDictionary<AssetId, TActionsArray> AssembleParameterActions(IPluginAtlas pluginAtlas) {
        Dictionary<AssetId, TActionsArray> actionMap = new();

        foreach (AssetRegistration registration in pluginAtlas.GetAssetRegistrations()) {
            ParameterInfo[] parameters = registration.Constructor.GetParameters();

            var actions = new Func<object>[parameters.Length];

            // Parse the parameter-info's and apply correct behaviour
            for (int index = 0; index < parameters.Length; index++) {
                ParameterInfo parameter = parameters[index];
                actions[index] = parameter switch {
                    _ when IsBasicNexitiesAsset(parameter) => () => CreateBasicNexitiesAsset(parameter),
                    _ when IsInjectedInstanceNexitiesAsset(parameter) => () => CreateInjectedInstanceNexitiesAsset(parameter),
                    _ => () => CreateUnknown(parameter)
                };
            }

            actionMap.Add(registration.AssetId, actions);
        }

        return actionMap.ToFrozenDictionary();
    }
    private static FrozenDictionary<Type, TConstructor> AssembleConstructorDelegates(IPluginAtlas pluginAtlas) {
        Dictionary<Type, TConstructor> constructorCache = new();

        foreach (AssetRegistration registration in pluginAtlas.GetAssetRegistrations()) {
            ConstructorInfo constructorInfo = registration.Constructor;
            ParameterExpression parametersArray = Expression.Parameter(typeof(object[]), "args");

            Expression[] parameterExpressions = constructorInfo.GetParameters().Select(
                (param, index) => Expression.Convert(
                    Expression.ArrayIndex(parametersArray, Expression.Constant(index)),
                    param.ParameterType)
            ).ToArray<Expression>();

            NewExpression newExpression = Expression.New(constructorInfo, parameterExpressions);
            Expression<TConstructor> lambda = Expression.Lambda<TConstructor>(Expression.Convert(newExpression, typeof(object)), parametersArray);

            constructorCache.Add(registration.Type, lambda.Compile());
        }

        return constructorCache.ToFrozenDictionary();
    }
    #endregion
    #region Basic Nexities Asset
    private static bool IsBasicNexitiesAsset(ParameterInfo parameter) {
        Type paramType = parameter.ParameterType;
        return typeof(IAssetInstance).IsAssignableFrom(paramType)
            && !parameter.GetCustomAttributes<ResolveAsSpecificAttribute>().Any()
            ;
    }
    private static object CreateBasicNexitiesAsset(ParameterInfo parameter) {
        IAssetInstanceAtlas instanceAtlas = EngineServices.GetAssetInstanceAtlas();

        return instanceAtlas.GetOrCreate<IAssetInstance>(parameter.ParameterType);
    }
    #endregion
    #region Injected Instance Nexities Asset
    private static bool IsInjectedInstanceNexitiesAsset(ParameterInfo parameter) {
        Type paramType = parameter.ParameterType;
        return typeof(IAssetInstance).IsAssignableFrom(paramType)
            && parameter.GetCustomAttributes<ResolveAsSpecificAttribute>().Any()
            ;
    }
    private static object CreateInjectedInstanceNexitiesAsset(ParameterInfo parameter) {
        IAssetInstanceAtlas instanceAtlas = EngineServices.GetAssetInstanceAtlas();
        var injectAsValue = parameter.GetCustomAttribute<ResolveAsSpecificAttribute>()!;
        Type paramType = parameter.ParameterType;

        return instanceAtlas.GetOrCreate<IAssetInstance>(paramType, injectAsValue.Ulid);
    }
    #endregion
}
