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
#if !DEBUG
using System.Security;
#endif

namespace AterraCore.OmniVault.Assets;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class AssetInstanceFactory(ILogger logger) : IAssetInstanceFactory {
    private readonly ConcurrentDictionary<AssetId, Func<object>[]> _actionsMap = new();
    private readonly ConcurrentDictionary<Type, Func<object[], object>> _constructorCache = new();
    
    // -----------------------------------------------------------------------------------------------------------------
    // Helper Methods
    // -----------------------------------------------------------------------------------------------------------------
    private static Func<object>[] CreateParameterActions(AssetRegistration registration) {
        ParameterInfo[] parameters = registration.Constructor.GetParameters();
        
        var actions = new Func<object>[parameters.Length]; // Actions is set to an empty array
        if (actions.Length <= 0) return actions;
        
        // Parse the parameter-info's and apply correct behaviour
        for (int index = 0; index < parameters.Length; index++) {
            ParameterInfo parameter = parameters[index];
            Func<object>? action;
            
            if      (IsBasicNexitiesAsset(parameter))             action = () => CreateBasicNexitiesAsset(parameter);
            else if (IsInjectedInstanceNexitiesAsset(parameter))  action = () => CreateInjectedInstanceNexitiesAsset(parameter);
            else if (IsReferencedAssetIdNexitiesAsset(parameter)) action = () => CreateReferencedAssetIdNexitiesAsset(parameter);

            else                                                  action = () => CreateUnknown(parameter);
            
            actions[index] = action ?? throw new ArgumentException($"Parameter could not be created - {parameter}");
        }

        // Finally try and store the actions
        return actions;
    }
    
    private static Func<object[], object> CreateConstructorDelegate(ConstructorInfo constructorInfo) {
        ParameterExpression parametersArray = Expression.Parameter(typeof(object[]), "args");
        
        Expression[] parameterExpressions = constructorInfo.GetParameters().Select(
            (param, index) => Expression.Convert(
                Expression.ArrayIndex(parametersArray, Expression.Constant(index)),
                param.ParameterType)
        ).ToArray<Expression>();
        
        NewExpression newExpression = Expression.New(constructorInfo, parameterExpressions);
        Expression<Func<object[], object>> lambda = Expression.Lambda<Func<object[], object>>(Expression.Convert(newExpression, typeof(object)), parametersArray);
        return lambda.Compile();
    }

    private readonly ArrayPool<object> _parameterPool = ArrayPool<object>.Shared;
    public bool TryCreate<T>(AssetRegistration registration, Ulid predefinedUlid, [NotNullWhen(true)] out T? assetInstance) where T : class, IAssetInstance {
        assetInstance = null;
        object[] parameters = _parameterPool.Rent(registration.Constructor.GetParameters().Length);
        
        try {
            Func<object[], object> constructorDelegate = _constructorCache.GetOrAdd(
                    registration.Type,
                    ConstructorFactory
                );
            
            // Pass the registration directly to avoid closure
            Func<object>[] parameterActions = _actionsMap.GetOrAdd(
                registration.AssetId,
                ParameterFactory
            );
            
            for (int i = 0; i < parameterActions.Length; i++) {
                parameters[i] = parameterActions[i]();
            }
            
            if (constructorDelegate(parameters) is not T castedInstance) return false;
            assetInstance = castedInstance;
            assetInstance.AssetId = registration.AssetId;
            assetInstance.InstanceId = predefinedUlid;
            
            return true;
        }
        
        // TODO ADD LOGGING
        catch (MethodAccessException e)         {logger.Error(e, "Caught MethodAccessException");return false;}
        catch (ArgumentException e)             {logger.Error(e, "Caught ArgumentException");return false;}
        catch (TargetInvocationException e)     {logger.Error(e, "Caught TargetInvocationException");return false;}
        catch (TargetParameterCountException e) {logger.Error(e, "Caught TargetParameterCountException");return false;}
        catch (NotSupportedException e)         {logger.Error(e, "Caught NotSupportedException");return false;}
        catch (SecurityException e)             {logger.Error(e, "Caught SecurityException");return false;}
        catch (Exception e)                     {logger.Error(e, "Caught unhandled error");return false;}
        
        finally {
            // Return the array to the pool
            _parameterPool.Return(parameters, clearArray: true);
        }

        Func<object[], object> ConstructorFactory(Type _) => CreateConstructorDelegate(registration.Constructor);
        Func<object>[] ParameterFactory(AssetId _) => CreateParameterActions(registration);
    }
    
    public T Create<T>(AssetRegistration registration, Ulid predefinedUlid) where T : class, IAssetInstance {
        Func<object[], object> constructorDelegate = _constructorCache.GetOrAdd(
            registration.Type,
            _ => CreateConstructorDelegate(registration.Constructor)
        );
            
        object[] parameters = _actionsMap
            .GetOrAdd(registration.AssetId, _ => CreateParameterActions(registration))
            .Select(action => action())
            .ToArray();

        var assetInstance = (T)constructorDelegate(parameters) ;

        assetInstance.AssetId = registration.AssetId;
        assetInstance.InstanceId = predefinedUlid;

        return assetInstance;
    }
    // -----------------------------------------------------------------------------------------------------------------
    // Parameter Creation Methods
    // -----------------------------------------------------------------------------------------------------------------

    #region Not a Nexities Asset
    private static object CreateUnknown(ParameterInfo parameter) {
        return parameter.ParameterType.IsInterface && EngineServices.TryGetService(parameter.ParameterType, out object? output) 
            ? output 
            : EngineServices.CreateWithServices<object>(parameter.ParameterType);
    }
    #endregion
    
    #region Basic Nexities Asset
    private static bool IsBasicNexitiesAsset(ParameterInfo parameter) {
        Type paramType = parameter.ParameterType;
        return typeof(IAssetInstance).IsAssignableFrom(paramType)
               && !parameter.GetCustomAttributes<InjectAsAttribute>().Any()
               && !parameter.GetCustomAttributes<ReferenceAsAttribute>().Any()
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
        
        return instanceAtlas.GetOrCreate<IAssetInstance>(parameter.ParameterType, injectAsValue.Ulid);
    }
    #endregion

    #region Referenced Asset Id Nexities Asset
    private static bool IsReferencedAssetIdNexitiesAsset(ParameterInfo parameter) {
        Type paramType = parameter.ParameterType;
        return typeof(IAssetInstance).IsAssignableFrom(paramType)
               && parameter.GetCustomAttributes<ReferenceAsAttribute>().Any()
            ;
    }
    private static object CreateReferencedAssetIdNexitiesAsset(ParameterInfo parameter) {
        IAssetInstanceAtlas instanceAtlas = EngineServices.GetAssetInstanceAtlas();
        IAssetAtlas assetAtlas = EngineServices.GetAssetAtlas();
        
        var referenceAs = parameter.GetCustomAttribute<ReferenceAsAttribute>()!;
        Type assetType = assetAtlas.GetAssetType(referenceAs.AssetId);
        
        return instanceAtlas.GetOrCreate<IAssetInstance>(assetType);
    }
    #endregion
}
