// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Frameworks.Nexities;
using System.Numerics;

namespace Workfloor.Entities;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
// [EntityFlags(PreStash | ...)]
// [Tag("workfloor_test:entities/box")]
public partial class BoxEntity : NexitiesEntity{
    [AsSpecific("GUID")] public partial ITransformComponent Transform { get; private set; }
    public ITransformComponent Transform2 { get; private set; }
}

















// AUTO GENERATED STUFF
public partial class BoxEntity {  
    // Might be a smart idea to keep a ref to the component service
    private IComponentAtlas _componentAtlas = EngineServices.GetRequiredService<IComponentAtlas>();
    
    // Make a ConcurrentTypedValueStore
    private ConcurrentTypedValueStore _componentStorage = [];
    
    #region Transform
    private ITransformComponent? _transform;
    public partial ITransformComponent Transform { get => GetComponentTransform(); private set => SetComponentTransform(value); }

    private ITransformComponent GetComponentTransform() {
        // Get from direct or indirect cache
        if (_transform is not null) return _transform;
        if (_componentStorage.TryGet<ITransformComponent>([NotNullWhen(true)] out ITransformComponent? _cached)) {
            return _transform = _cached;
        }

        SetComponentTransform(); // Ensures that _transform is set, or it throws
        return _transform!;
    }
    
    private void SetComponentTransform(ITransformComponent? value = null) {
        value ??= _componentAtlas.GetComponent<ITransformComponent>(specificGuid: "GUID".ToGuid());

        // Maybe only do this in debug mode
        if (!_componentStorage.HasAvailableSlot<ITransformComponent>()) {
            throw new Exception("No available slots for ITransformComponent type"); // Each type can only have one component of that type.
        }

        if (_transform is not null) CleanupComponentTransform();
        
        _transform = value;
        _componentStorage.Store<ITransformComponent>(_transform);
    }

    private void CleanupComponentTransform() {
        _componentStorage.Remove<ITransformComponent>();
        _componentAtlas.returnComponent(Transform);
    }
    #endregion
    
    public override void Initialize() {
        // base.Initialize(); // Should only be the case for non abstract base types.
        Transform = _componentAtlas.GetComponent<ITransformComponent>(specificGuid : "GUID".ToGuid());
        Transform2 = _componentAtlas.GetComponent<ITransformComponent>();
    }

    public override void Cleanup() {
        CleanupComponentTransform();
        _componentAtlas.returnComponent(Transform2);

        _componentStorage.Clear();
    }
}

public interface ITransformComponent {
    Vector2 Position { get; set; }
    Vector2 Size { get; set; }
    float Rotation { get; set; }
    float Scale { get; set; }
}

[Tag("workfloor_test:components/transform")]
public class TransformComponent : NexitiesComponent {
    public Vector2 Position { get; set; } = Vector2.Zero;
    public Vector2 Size { get; set; } = Vector2.Zero;
    public float Rotation { get; set; }
    public float Scale { get; set; }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override void Cleanup() {
        Position = Vector2.Zero;
        Size = Vector2.Zero;
        Rotation = 0;
        Scale = 1;
    }
}