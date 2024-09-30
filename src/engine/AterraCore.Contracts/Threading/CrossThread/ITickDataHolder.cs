// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.OmniVault.Assets;

namespace AterraCore.Contracts.Threading.CrossThread;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ITickDataHolder : IAssetInstance {
    bool IsEmpty { get; }
    void ClearOnLevelChange();
    void ClearOnLogicTick();
    void ClearOnRenderFrame();
}
