// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Frameworks.Nexities;
using AterraEngine.Frameworks.Omnia;

namespace Workfloor.AterraEngine.Frameworks.Nexities.Generators;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[OmniaId("example:entity")]
public partial class Entity : NexitiesEntity {
    public partial ExampleComponent ExampleComponent { get; }
    
    [AsSpecifcId("some guid")] public partial ISomeComponent SomeComponent { get; }
}

public interface ISomeComponent : INexitiesComponent {}

// Below this point should be auto generated code
// We shouldn't touch any of thi, but it should overrideable? Don't know about that yet
// Technically if I enabled lang=preview I could use the "field" of an auto property, but given it is still in preview.
public partial class Entity {
    private ExampleComponent? _exampleComponent;
    public partial ExampleComponent ExampleComponent {
        get {
            if (_exampleComponent is not null) return _exampleComponent;
            if (TryGetComponent<ExampleComponent>(out ExampleComponent? cached)) return _exampleComponent = cached;
            throw new Exception("Could not find ExampleComponent");
        }
    }
    
    private ISomeComponent? _someComponent;
    public partial ISomeComponent SomeComponent {
        get {
            if (_someComponent is not null) return _someComponent;
            // TODO : remvoe the OmniaId paramater because it shouldnt be needed to get a component.
            if (TryGetComponent<ISomeComponent>(out ISomeComponent? cached)) return _someComponent = cached;
            throw new Exception("Could not find ExampleComponent");
        }
    }

    public override void Initialize(OmniaId assetId) {
        base.Initialize(assetId);
        
        // TODO inject this or go through "EngineServices"
        var library = new OmniaLibrary();
        
        // Per component we try and fetch it and then try and register it to the omnia library and the internal lookup for the entity 
        if (library.CreateAsset<ExampleComponent>() is not {IsT: true, AsT: {} exampleComponent }) throw new Exception("Could not create ExampleComponent");
        if (!TryRegisterComponent(exampleComponent)) throw new Exception("Could not register ExampleComponent");
        
        if (library.FindOrCreateAsset<ISomeComponent>("some guid") is not {IsT: true, AsT: {} someComponent }) throw new Exception("Could not create ExampleComponent");
        if (!TryRegisterComponent(someComponent)) throw new Exception("Could not register ExampleComponent");
    }
}