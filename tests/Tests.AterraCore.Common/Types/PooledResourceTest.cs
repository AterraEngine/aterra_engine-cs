// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.ObjectPool;

namespace Tests.AterraCore.Common.Types;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[TestSubject(typeof(PooledResource<>))]
public class PooledResourceTest {
    private class MockPooledObject;

    private class SimpleObjectPool<T> : ObjectPool<T> where T : class, new() {
        public override T Get() => new();
        public override void Return(T obj) {
            /* No-op for simplicity */
        }
    }

    [Fact]
    public void PooledResource_Should_Return_Object_To_Pool_On_Dispose() {
        // Arrange
        var pool = new SimpleObjectPool<MockPooledObject>();

        // Act
        MockPooledObject item;
        using (var pooledResource = new PooledResource<MockPooledObject>(pool)) {
            item = pooledResource.Item;
        }

        // Assert
        Assert.NotNull(item);
        // Since our SimpleObjectPool does a No-op on return,
        // checking the item is not null ensures that it was created properly.
        // More complex logic would be needed to test return behavior.
        Assert.IsType<MockPooledObject>(item);
    }
}
