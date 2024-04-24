// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;
using AterraEngine.Contracts.Lib.ECS.Components;
using AterraEngine.Core.ECSFramework;
using AterraEngine.Lib.ECS.Dtos.Components;
using Serilog;

namespace AterraEngine.Lib.ECS.Components;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
// [Asset<Transform2DDto>(DefaultComponents.PTransform2D, AssetType.ECSComponent)]
public class Transform2D(ILogger logger) : Component<Transform2DDto>(logger), ITransform2D{
    private float _rotation;
    public float Rotation {
        get => _rotation;
        set {_rotation = value; _transformationMatrix = null; }
    }
    
    private Vector2 _position;
    public Vector2 Position {
        get => _position;
        set {_position = value; _transformationMatrix = null; }
    }
    
    private Vector2 _scale = Vector2.One;
    public Vector2 Scale {
        get => _scale;
        set {_scale = value; _transformationMatrix = null; }
    }

    private Matrix3x2? _transformationMatrix;
    public Matrix3x2 TransformationMatrix {
        get {
            if (_transformationMatrix != null) return (Matrix3x2)_transformationMatrix;
            
            var translationMatrix = Matrix3x2.CreateTranslation(Position);
            var rotationMatrix = Matrix3x2.CreateRotation(Rotation, Position);
            var scaleMatrix = Matrix3x2.CreateScale(Scale, Position);
            _transformationMatrix = scaleMatrix * rotationMatrix * translationMatrix;
            return (Matrix3x2)_transformationMatrix;
        }
    }
    
    public override void PopulateFromDto(Transform2DDto dto) {
        Rotation = dto.Rotation ?? 0f;
        Position = dto.Position ?? Vector2.Zero;
        Scale = dto.Scale ?? Vector2.One;
    }
}