// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Threading.Rendering;
using JetBrains.Annotations;

namespace AterraEngine.Threading.Render;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class RenderEventManager : IRenderEventManager {
    public event EventHandler? EventStart;
    public event EventHandler? EventStop;
    public event EventHandler? EventWindowResized; 
    public event EventHandler? EventClearSystemCaches;
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void InvokeStop() => EventStop?.Invoke(this, EventArgs.Empty);
    public void InvokeStart() => EventStart?.Invoke(this, EventArgs.Empty);
    public void InvokeWindowResized() => EventWindowResized?.Invoke(this, EventArgs.Empty);
    public void InvokeClearSystemCaches() => EventClearSystemCaches?.Invoke(this, EventArgs.Empty);
}
