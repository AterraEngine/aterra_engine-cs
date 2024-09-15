// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Attributes;
using AterraCore.Contracts.ConfigMancer;
using AterraCore.Contracts.Renderer;
using JetBrains.Annotations;

namespace AterraEngine.Threading.Render;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
[Singleton<IMainWindow>]
public class MainWindow(IConfigAtlas configAtlas) : IMainWindow {
    public void Init() {
        // Necessary to write Raylib logs with Serilog
        unsafe { Raylib.SetTraceLogCallback(RaylibLogger.GetPointer()); }

        Raylib.SetConfigFlags(
            ConfigFlags.ResizableWindow
            | ConfigFlags.Msaa4xHint// Enable Multi Sampling Anti Aliasing 4x (if available)
            // | ConfigFlags.InterlacedHint
            // | ConfigFlags.UndecoratedWindow
            // | ConfigFlags.MousePassthroughWindow
            // | ConfigFlags.TransparentWindow
        );

        Raylib.InitWindow(
            configAtlas.EngineConfigXml.Window.Width,
            configAtlas.EngineConfigXml.Window.Height,
            configAtlas.EngineConfigXml.Window.Name
        );

        Raylib.SetWindowMonitor(configAtlas.EngineConfigXml.Window.MonitorId);

    }
}
