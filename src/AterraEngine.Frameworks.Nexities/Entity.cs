// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Buffers;

namespace AterraEngine.Frameworks.Nexities;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class NexitiesEntity(int initialCapacity = 0) {
    public NexitiesComponent[] Components { get; private set; } = ArrayPool<NexitiesComponent>.Shared.Rent(initialCapacity);
    public uint ComponentCount { get; private set; }
    
    private void ResizeComponentsIfNeeded(int newSize) {
        if (newSize <= Components.Length) return;
        
        var newComponents = ArrayPool<NexitiesComponent>.Shared.Rent(newSize);
        Components.CopyTo(newComponents, 0);
        Components = newComponents;
    }

    protected void SetComponent(NexitiesComponent component, int index) {
        ResizeComponentsIfNeeded(index + 1);
        Components[index] = component;
        ComponentCount++;
    }
    
    protected void SetComponent<TComponent>(int index) where TComponent : NexitiesComponent, new() {
        ResizeComponentsIfNeeded(index + 1);
        Components[index] = new TComponent();
        ComponentCount++;
    }
}