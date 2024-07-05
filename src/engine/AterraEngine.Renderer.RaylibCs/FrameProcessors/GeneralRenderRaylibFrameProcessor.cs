// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.FlexiPlug;
using AterraCore.Contracts.Nexities.Data.Assets;
using AterraCore.Nexities.Lib.Entities.Actor;
using AterraCore.Nexities.Lib.Systems;
using JetBrains.Annotations;
using Serilog;
using System.Numerics;

namespace AterraEngine.Renderer.RaylibCs.FrameProcessors;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class GeneralRenderRaylibFrameProcessor(IAssetInstanceAtlas assetInstanceAtlas, IPluginAtlas pluginAtlas, ILogger logger) : AbstractRaylibFrameProcessor {
    protected override Color ClearColor { get; set; } = new(0, 0, 0, 0);
    private Render2D Render2D { get; } = new(logger);
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override void Draw2D() {
        if (assetInstanceAtlas.TryGet(Guid.Parse("af15db3d-f69e-4382-a768-d163011125f5"), out IActor2D? actor2D)) {
            Render2D.ProcessSingularEntity(actor2D);
        }
    }
}
