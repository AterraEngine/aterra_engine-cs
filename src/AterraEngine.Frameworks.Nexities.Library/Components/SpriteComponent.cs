// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using Raylib_cs;

namespace AterraEngine.Frameworks.Nexities.Library.Components;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public sealed partial class SpriteComponent : NexitiesComponent<TransformComponent>, ISpriteComponent {
    public string TextureId { get; set; } = string.Empty;
    public Color Tint { get; set; } = Color.White;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override bool TryReset() {
        if (!base.TryReset()) return false;
        TextureId = string.Empty;
        Tint = Color.White;
        return true;
    }
}