// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using Extensions;
using Raylib_cs;

namespace AterraCore.Nexities.Lib.Systems;

using Common.Types.Nexities;
using Components.HUD;
using Components.HUD.Text;
using Entities.HUD;
using Nexities.Systems;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[System("AF00-0001", ServiceLifetimeType.Singleton, CoreTags.RenderSystem)]
public class RenderHud : NexitiesSystem<IHud> {
    protected override void ProcessEntity(IHud entity) {
        entity.ChildEntities
            .OfTypeManyReverse<IHudElement>()
            .IterateOver(element => {
                switch (element.HudComponent.Type) {
                    case HudType.Text when element.HudComponent is RaylibHudText text: {
                        Raylib.DrawText(text.Text, text.Pos[0], text.Pos[1], text.FontSize, text.Color);
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