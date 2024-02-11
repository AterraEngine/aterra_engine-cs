// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Contracts.Core.ECSFramework.Events;
using AterraEngine.Contracts.Delegates;
namespace AterraEngine.Core.ECSFramework.Events;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class EventManager : IEventManager {
    private Dictionary<Type, Delegate> _eventHandlers = new ();

    public void Register<TEvent>(EventCallback<TEvent> handler) where TEvent : IEvent {
        _eventHandlers[typeof(TEvent)] = handler;
    }

    public void Unregister<TEvent>() where TEvent : IEvent {
        _eventHandlers.Remove(typeof(TEvent));
    }

    public void Trigger<TEvent>(TEvent e) where TEvent : IEvent {
        if (_eventHandlers.TryGetValue(typeof(TEvent), out Delegate? handler)) {
            ((EventCallback<TEvent>)handler)(e);
        }
        // TODO logging
    }
} 