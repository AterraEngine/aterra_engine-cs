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
        entity.ChildEntities
            .OfTypeManyReverse<IHudElement>()
            .IterateOver(element => {
                switch (element.HudComponent.Type) {
                    case HudType.Text when element.HudComponent is RaylibHudText text : {
                        Raylib.DrawText(text.Text, text.Pos.X, text.Pos.Y, text.FontSize, text.Color);
                        break;
                    }

                    case HudType.TextPro: {
                        break;
                    }
                    
                    case HudType.Texture:{
                        break;
                    }
                    
                    case HudType.Empty:
                    default: {
                        break;
                    }
                }
            });
    }
}