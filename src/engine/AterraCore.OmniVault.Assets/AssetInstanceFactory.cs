// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Attributes;
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.OmniVault.Assets;
using AterraCore.DI;
using JetBrains.Annotations;
using Serilog;
using System.Buffers;
using System.Collections.Concurrent;
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
public class AssetInstanceFactory(ILogger logger) : IAssetInstanceFactory {
    private readonly ConcurrentDictionary<AssetId, TActionsArray> _actionsMap = new();
    private readonly ConcurrentDictionary<Type, TConstructor> _constructorCache = new();
    private static readonly ArrayPool<object> ParameterPool = ArrayPool<object>.Shared;

    // -----------------------------------------------------------------------------------------------------------------
    // Helper Methods
    // -----------------------------------------------------------------------------------------------------------------
    private static TActionsArray CreateParameterActions(AssetRegistration registration) {
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

        return actions;
    }

    private static TConstructor CreateConstructorDelegate(ConstructorInfo constructorInfo) {
        ParameterExpression parametersArray = Expression.Parameter(typeof(object[]), "args");

        Expression[] parameterExpressions = constructorInfo.GetParameters().Select(
            (param, index) => Expression.Convert(
                Expression.ArrayIndex(parametersArray, Expression.Constant(index)),
                param.ParameterType)
        ).ToArray<Expression>();

        NewExpression newExpression = Expression.New(constructorInfo, parameterExpressions);
        Expression<TConstructor> lambda = Expression.Lambda<TConstructor>(Expression.Convert(newExpression, typeof(object)), parametersArray);
        return lambda.Compile();
    }

    public bool TryCreate<T>(AssetRegistration registration, Ulid predefinedUlid, [NotNullWhen(true)] out T? assetInstance) where T : class, IAssetInstance {
        assetInstance = null;
        object[] parameters = ParameterPool.Rent(registration.Constructor.GetParameters().Length);

        try {
            TConstructor constructorDelegate = _constructorCache.GetOrAdd(registration.Type, valueFactory: _ => CreateConstructorDelegate(registration.Constructor));
            TActionsArray parameterActions = _actionsMap.GetOrAdd(registration.AssetId, valueFactory: _ => CreateParameterActions(registration));

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
        TConstructor constructorDelegate = _constructorCache.GetOrAdd(
            registration.Type,
            valueFactory: _ => CreateConstructorDelegate(registration.Constructor)
        );

        object[] parameters = _actionsMap
            .GetOrAdd(registration.AssetId, valueFactory: _ => CreateParameterActions(registration))
            .Select(action => action())
            .ToArray();

        var assetInstance = (T)constructorDelegate(parameters);

        assetInstance.AssetId = registration.AssetId;
        assetInstance.InstanceId = predefinedUlid;

        return assetInstance;
    }
    // -----------------------------------------------------------------------------------------------------------------
    // Parameter Creation Methods
    // -----------------------------------------------------------------------------------------------------------------

    #region Not a Nexities Asset
    private static object CreateUnknown(ParameterInfo parameter) {
        Type paramType = parameter.ParameterType;
        return paramType.IsInterface && EngineServices.TryGetService(paramType, out object? output)
            ? output
            : EngineServices.CreateWithServices<object>(paramType);
    }
    #endregion

    #region Basic Nexities Asset
    private static bool IsBasicNexitiesAsset(ParameterInfo parameter) {
        Type paramType = parameter.ParameterType;
        return typeof(IAssetInstance).IsAssignableFrom(paramType)
               && !parameter.GetCustomAttributes<InjectAsAttribute>().Any()
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
               && parameter.GetCustomAttributes<InjectAsAttribute>().Any()
            ;
    }
    private static object CreateInjectedInstanceNexitiesAsset(ParameterInfo parameter) {
        IAssetInstanceAtlas instanceAtlas = EngineServices.GetAssetInstanceAtlas();
        var injectAsValue = parameter.GetCustomAttribute<InjectAsAttribute>()!;
        Type paramType = parameter.ParameterType;

        return instanceAtlas.GetOrCreate<IAssetInstance>(paramType, injectAsValue.Ulid);
    }
    #endregion
}
