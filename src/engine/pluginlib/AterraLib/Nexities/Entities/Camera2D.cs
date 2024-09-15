// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Attributes;

namespace AterraLib.Nexities.Entities;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Serializable]
[UsedImplicitly]
[Entity<IActor2D, ICamera2D>(StringAssetIdLib.AterraLib.Entities.Camera2D)]
public class Camera2D(IImpulse2D impulse2D, IRaylibCamera2D raylibCamera2D) : NexitiesEntity(impulse2D, raylibCamera2D), ICamera2D {
    private IImpulse2D? _impulse2D = impulse2D;

    private IRaylibCamera2D? _raylibCamera2D = raylibCamera2D;
    public IImpulse2D Impulse2D => _impulse2D ??= GetComponent<IImpulse2D>();
    public IRaylibCamera2D RaylibCamera2D => _raylibCamera2D ??= GetComponent<IRaylibCamera2D>();

    protected override void ClearCaches() {
        _impulse2D = null;
        _raylibCamera2D = null;
    }
}
