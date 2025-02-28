// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.DependencyInjection;
using System.Collections.Concurrent;
using System.Collections.Frozen;
using System.Collections.Immutable;

namespace AterraEngine.Frameworks.Continuum;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ContinuumServiceFactory(IServiceCollection serviceCollection) : IContinuumServiceFactory {
    private readonly Lock _configuredLock = new();
    private ConcurrentDictionary<Type, ICommandHubBuilder> _commandHubs = [];

    private bool _isConfigured;
    private ConcurrentDictionary<Type, IQueryHubBuilder> _queryHubs = [];
    private ConcurrentDictionary<Type, ITriggerHubBuilder> _triggerHubs = [];

    private ImmutableDictionary<Type, ICommandHubBuilder> CommandHubs { get; set; } = ImmutableDictionary<Type, ICommandHubBuilder>.Empty;
    private ImmutableDictionary<Type, ITriggerHubBuilder> TriggerHubs { get; set; } = ImmutableDictionary<Type, ITriggerHubBuilder>.Empty;
    private ImmutableDictionary<Type, IQueryHubBuilder> QueryHubs { get; set; } = ImmutableDictionary<Type, IQueryHubBuilder>.Empty;
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public IContinuum Create(IScopedProvider provider) {
        ConfigureContinuumServiceIfRequired();

        var bus = new ContinuumService {
            CommandHubs = CommandHubs.ToFrozenDictionary(
                keySelector: kvp => kvp.Key,
                elementSelector: kvp => kvp.Value.BuildHub(provider)),
            TriggerHubs = TriggerHubs.ToFrozenDictionary(
                keySelector: kvp => kvp.Key,
                elementSelector: kvp => kvp.Value.BuildHub(provider)),
            QueryHubs = QueryHubs.ToFrozenDictionary(
                keySelector: kvp => kvp.Key,
                elementSelector: kvp => kvp.Value.BuildHub(provider))
        };

        bus.StartProcessing();
        return bus;
    }

    public ICommandHubBuilder<TCommand, TResult> AddCommand<TCommand, TResult>()
        where TCommand : ICommand<TResult>
        where TResult : struct {
        ThrowIfConfigured();

        ICommandHubBuilder builder = _commandHubs.GetOrAdd(
            typeof(TCommand),
            valueFactory: static (_, sc) => new CommandHubBuilder<TCommand, TResult>(sc),
            serviceCollection
        );

        if (builder is not ICommandHubBuilder<TCommand, TResult> typedBuilder) throw new InvalidOperationException("Failed to get command builder");

        return typedBuilder;
    }


    public ITriggerHubBuilder<TTrigger> AddTrigger<TTrigger>() where TTrigger : ITrigger {
        ThrowIfConfigured();

        ITriggerHubBuilder builder = _triggerHubs.GetOrAdd(
            typeof(TTrigger),
            valueFactory: static (_, sc) => new TriggerHubBuilder<TTrigger>(sc),
            serviceCollection
        );

        if (builder is not ITriggerHubBuilder<TTrigger> typedBuilder) throw new InvalidOperationException("Failed to get typed builder");

        return typedBuilder;
    }

    public IQueryHubBuilder<TQuery, TResult> AddQuery<TQuery, TResult>() where TQuery : IQuery<TResult> where TResult : struct {
        ThrowIfConfigured();

        IQueryHubBuilder builder = _queryHubs.GetOrAdd(
            typeof(TQuery),
            valueFactory: static (_, sc) => new QueryHubBuilder<TQuery, TResult>(sc),
            serviceCollection
        );

        if (builder is not IQueryHubBuilder<TQuery, TResult> typedBuilder) throw new InvalidOperationException("Failed to get typed builder");

        return typedBuilder;
    }

    private void ConfigureContinuumServiceIfRequired() {
        lock (_configuredLock) {
            if (_isConfigured) return;

            CommandHubs = _commandHubs.ToImmutableDictionary();
            TriggerHubs = _triggerHubs.ToImmutableDictionary();
            QueryHubs = _queryHubs.ToImmutableDictionary();

            // Don't keep a reference if we don't need it anymore
            _commandHubs = null!;
            _triggerHubs = null!;
            _queryHubs = null!;

            _isConfigured = true;
        }
    }

    private void ThrowIfConfigured() {
        if (_isConfigured) throw new InvalidOperationException("Cannot configure a message bus factory after it has been used");
    }
}
