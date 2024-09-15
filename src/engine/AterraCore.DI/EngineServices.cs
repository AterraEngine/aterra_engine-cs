// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Contracts;
using AterraCore.Contracts.FlexiPlug;
using AterraCore.Contracts.OmniVault.Assets;
using CodeOfChaos.Extensions.Serilog;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Core;
using System.Diagnostics.CodeAnalysis;

namespace AterraCore.DI;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
///     Provides various engine-related services such as dependency injection, logging, and asset management.
/// </summary>
public static class EngineServices {
    private static IServiceCollection? _serviceCollection;

    /// <summary>
    ///     Represents the logger used in the EngineServices class.
    /// </summary>
    private static ILogger? _logger;

    /// <summary>
    ///     Provides access to various services in the AterraCore Engine.
    /// </summary>
    private static ServiceProvider ServiceProvider { get; set; } = null!;

    /// <summary>
    ///     Provides logging functionality for the application.
    /// </summary>
    private static ILogger Logger => _logger ??= GetLogger().ForContext(Constants.SourceContextPropertyName, typeof(EngineServices));

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Builds the service provider using the provided service collection.
    /// </summary>
    /// <param name="serviceCollection">The service collection.</param>
    public static void BuildServiceProvider(IServiceCollection serviceCollection) {
        ServiceProvider = serviceCollection.BuildServiceProvider();
        _serviceCollection = serviceCollection;
    }

    /// <summary>
    ///     Retrieves an instance of the specified service type.
    /// </summary>
    /// <typeparam name="T">The type of the service to retrieve.</typeparam>
    /// <returns>An instance of the specified service type.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the service type could not be found.</exception>
    public static T GetService<T>() where T : notnull {
        try {
            return ServiceProvider.GetRequiredService<T>();
        }
        catch (InvalidOperationException e) {
            Type type = typeof(T);

            ServiceDescriptor? serviceDescriptor = _serviceCollection?.FirstOrDefault(descriptor => descriptor.ServiceType == type);
            if (serviceDescriptor?.ImplementationType?.GetConstructors().FirstOrDefault() is not {} constructor) {
                throw Logger.ThrowFatal(e, "Service type of {TypeOfT} could not be found.", type.FullName);
            }

            IEnumerable<Type> paramTypes = constructor.GetParameters().Select(p => p.ParameterType);
            foreach (Type paramType in paramTypes) {
                if (ServiceProvider.GetService(paramType) is not null) continue;

                throw Logger.ThrowFatal(e, "Service type of {paramType} could not be found while resolving {TypeOfT}", paramType.FullName, type.FullName);
            }

            throw;
        }
    }

    public static bool TryGetService<T>([NotNullWhen(true)] out T? output) where T : class => TryGetService(typeof(T), out output);
    public static bool TryGetService<T>(Type input, [NotNullWhen(true)] out T? output) where T : class {
        output = ServiceProvider.GetService(input) as T;
        return output is not null;
    }

    /// <summary>
    ///     Creates an instance of the specified type with the required services.
    /// </summary>
    /// <typeparam name="T">The type of the instance to create.</typeparam>
    /// <returns>
    ///     An instance of the specified type.
    /// </returns>
    /// <remarks>
    ///     This method uses the <see cref="IServiceProvider" /> to resolve dependencies and create the instance of the
    ///     specified type.
    /// </remarks>
    public static T CreateWithServices<T>() => CreateWithServices<T>(typeof(T));
    /// <summary>
    ///     Creates an instance of type T with the registered services.
    /// </summary>
    /// <typeparam name="T">The type of the object to be created.</typeparam>
    /// <param name="objectType">The type of the object to be created.</param>
    /// <returns>The created instance of type T.</returns>
    public static T CreateWithServices<T>(Type objectType) => (T)ActivatorUtilities.CreateInstance(ServiceProvider, objectType);

    // -----------------------------------------------------------------------------------------------------------------
    // Default Systems Quick access
    // -----------------------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Retrieves the logger instance for the EngineServices context.
    /// </summary>
    /// <returns>The logger instance.</returns>
    public static ILogger GetLogger() => GetService<ILogger>();
    /// <summary>
    ///     Retrieves an instance of the <see cref="IEngine" /> interface.
    /// </summary>
    /// <returns>An instance of the <see cref="IEngine" /> interface.</returns>
    public static IEngine GetEngine() => GetService<IEngine>();
    /// <summary>
    ///     Retrieves the instance of the plugin atlas service.
    /// </summary>
    /// <returns>The instance of the plugin atlas service.</returns>
    public static IPluginAtlas GetPluginAtlas() => GetService<IPluginAtlas>();
    /// <summary>
    ///     Retrieves an instance of the asset atlas.
    /// </summary>
    /// <returns>An instance of the asset atlas.</returns>
    public static IAssetAtlas GetAssetAtlas() => GetService<IAssetAtlas>();
    /// <summary>
    ///     Retrieves the instance atlas for asset instances.
    /// </summary>
    /// <returns>The asset instance atlas.</returns>
    public static IAssetInstanceAtlas GetAssetInstanceAtlas() => GetService<IAssetInstanceAtlas>();
    /// <summary>
    ///     Retrieves the factory for instances of assets.
    /// </summary>
    /// <returns>The Asset Instance Factory.</returns>
    public static IAssetInstanceFactory GetAssetInstanceFactory() => GetService<IAssetInstanceFactory>();
}
