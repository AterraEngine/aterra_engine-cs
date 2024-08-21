// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraCore.Contracts.Threading.Rendering;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IRenderEventManager {
    public event EventHandler? EventStart;
    public event EventHandler? EventStop;
    public event EventHandler? EventWindowResized; 
    public event EventHandler? EventClearSystemCaches; 

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void InvokeStop();
    public void InvokeStart();
    public void InvokeWindowResized();
    public void InvokeClearSystemCaches();
}
