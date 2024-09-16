// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraCore.Contracts.Threading.Rendering;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IRenderThreadProcessor : IThreadProcessor {
    public event TickEventHandler? TickEvent2DMode;  
    public event TickEventHandler? TickEvent3DMode;  
    public event TickEventHandler? TickEventUiMode;  
}


