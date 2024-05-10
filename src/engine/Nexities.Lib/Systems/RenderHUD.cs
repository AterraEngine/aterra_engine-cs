// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Common.Nexities;
using AterraCore.Nexities.Systems;
using Extensions;
using Nexities.Lib.Components.HUD;
using Nexities.Lib.Components.HUD.Text;
using Nexities.Lib.Entities.HUD;
using Raylib_cs;
using Serilog;

namespace Nexities.Lib.Systems;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[System("AF00-0001", AssetInstanceType.Singleton, CoreTags.RenderSystem)]
public class RenderHUD(ILogger logger) : NexitiesSystem<IHud> {
    protected override void ProcessEntity(IHud entity) {
        entity.ChildEntities.OfTypeManyReverse<IHudElement>()
            .Select(element => new {element.HudComponent.Type,Component = element.HudComponent})
            .IterateOver(box => {
                switch (box.Type) {
                    case HudType.Text : {
                        if (box.Component is RaylibHudText text) 
                            Raylib.DrawText(text.Text, text.Pos.X, text.Pos.Y, text.FontSize, text.Color);
                        break;
                    }
                    case HudType.Empty:
                    case HudType.TextPro:
                    case HudType.Texture:
                    default: {
                        break;
                    }
                }
            });
    }
}