// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.OmniVault.World;
using AterraCore.Contracts.Threading.Logic;
using JetBrains.Annotations;

namespace AterraEngine.Renderer.RaylibCs.FrameProcessors;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class UndefinedRaylibFrameProcessor(IAterraCoreWorld world, ILogicEventManager eventManager) : AbstractRaylibFrameProcessor(world,eventManager) {
    protected override Color ClearColor { get; } = new(0, 0, 0, 0);

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
}
