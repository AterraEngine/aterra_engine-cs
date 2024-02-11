// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Contracts.Delegates;

namespace AterraEngine.Contracts.Core.ECSFramework.Events;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public interface IEventManager {
    void Register<TEvent>(EventCallback<TEvent> callback) where TEvent : IEvent;
    void Unregister<TEvent>() where TEvent : IEvent;
    void Trigger<TEvent>(TEvent e) where TEvent : IEvent;
}