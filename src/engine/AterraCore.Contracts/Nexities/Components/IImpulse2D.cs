// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;

namespace AterraCore.Contracts.Nexities.Components;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IImpulse2D : INexitiesComponent {
    Vector2 TranslationOffset { get; set; }
    Vector2 ScaleOffset { get; set; }
    float RotationOffset { get; set; }
    bool IsEmpty { get; }
    bool IsNotEmpty { get; }

    void Clear();
}
