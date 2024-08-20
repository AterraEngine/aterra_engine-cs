// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.OmniVault.Assets;
using AterraCore.Contracts.OmniVault.Assets.Attributes;
using AterraCore.DI;
using JetBrains.Annotations;
using Serilog;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;
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
    private Func<object>[] CreateParameterActions(AssetRegistration registration) {
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
        logger.Debug("Created new asset factory");
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

    public bool TryCreate<T>(AssetRegistration registration, Ulid predefinedUlid, [NotNullWhen(true)] out T? assetInstance) where T : class, IAssetInstance {
        assetInstance = null;
        #if !DEBUG
        try {
        #endif
            Func<object[], object> constructorDelegate = _constructorCache.GetOrAdd(
                registration.Type,
                _ => CreateConstructorDelegate(registration.Constructor)
            );
            
            object[] parameters = _actionsMap
                .GetOrAdd(registration.AssetId, _ => CreateParameterActions(registration))
                .Select(action => action())
                .ToArray();
            
            if (constructorDelegate(parameters) is not T castedInstance) return false;
            assetInstance = castedInstance;
            
            assetInstance.AssetId = registration.AssetId;
            assetInstance.InstanceId = predefinedUlid;
            
            return true;
        #if !DEBUG
        }
        
        // TODO ADD LOGGING
        catch (MethodAccessException e)         {logger.Error(e, "Caught MethodAccessException");}
        catch (ArgumentException e)             {logger.Error(e, "Caught ArgumentException");}
        catch (TargetInvocationException e)     {logger.Error(e, "Caught TargetInvocationException");}
        catch (TargetParameterCountException e) {logger.Error(e, "Caught TargetParameterCountException");}
        catch (NotSupportedException e)         {logger.Error(e, "Caught NotSupportedException");}
        catch (SecurityException e)             {logger.Error(e, "Caught SecurityException");}
        catch (Exception e)                     {logger.Error(e, "Caught unhandled error");}
        return false;
        #endif
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
               && !parameter.GetCustomAttributes<IInjectAsAttribute>().Any()
               && !parameter.GetCustomAttributes<IReferenceAsAttribute>().Any()
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
               && parameter.GetCustomAttributes<IInjectAsAttribute>().Any()
            ;
    }
    private static object CreateInjectedInstanceNexitiesAsset(ParameterInfo parameter) {
        IAssetInstanceAtlas instanceAtlas = EngineServices.GetAssetInstanceAtlas();
        var injectAsValue = parameter.GetCustomAttribute<IInjectAsAttribute>()!;
        
        return instanceAtlas.GetOrCreate<IAssetInstance>(parameter.ParameterType, injectAsValue.Ulid);
    }
    #endregion

    #region Referenced Asset Id Nexities Asset
    private static bool IsReferencedAssetIdNexitiesAsset(ParameterInfo parameter) {
        Type paramType = parameter.ParameterType;
        return typeof(IAssetInstance).IsAssignableFrom(paramType)
               && parameter.GetCustomAttributes<IReferenceAsAttribute>().Any()
            ;
    }
    private static object CreateReferencedAssetIdNexitiesAsset(ParameterInfo parameter) {
        IAssetInstanceAtlas instanceAtlas = EngineServices.GetAssetInstanceAtlas();
        IAssetAtlas assetAtlas = EngineServices.GetAssetAtlas();
        
        var referenceAs = parameter.GetCustomAttribute<IReferenceAsAttribute>()!;
        Type assetType = assetAtlas.GetAssetType(referenceAs.AssetId);
        
        return instanceAtlas.GetOrCreate<IAssetInstance>(assetType);
    }
    #endregion
}
