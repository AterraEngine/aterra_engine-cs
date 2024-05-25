// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Common.Nexities;
using Extensions;
using AterraCore.Nexities.Lib.Components.HUD;
using AterraCore.Nexities.Lib.Components.HUD.Text;
using AterraCore.Nexities.Lib.Entities.HUD;
using Raylib_cs;
using Serilog;
namespace AterraCore.Nexities.Lib.Systems;

using Components.HUD;
using Components.HUD.Text;
using Entities.HUD;
using Nexities.Systems;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[System("AF00-0001", ServiceLifetimeType.Singleton, CoreTags.RenderSystem)]
public class RenderHUD : NexitiesSystem<IHud> {
    protected override void ProcessEntity(IHud entity) {
        entity.ChildEntities
            .OfTypeManyReverse<IHudElement>()
            .IterateOver(element => {
                switch (element.HudComponent.Type) {
                    case HudType.Text when element.HudComponent is RaylibHudText text: {
                        Raylib.DrawText(text.Text, text.Pos.X, text.Pos.Y, text.FontSize, text.Color);
                        break;
                    }

                    case HudType.TextPro: {
                        break;
                    }

                    case HudType.Texture: {
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