// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraCore.Contracts.OmniVault.World.EntityTree;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IEntityTreeFactory {
    /// <summary>
    /// Creates an instance of IEntityNodeTree from the specified root instance ID.
    /// </summary>
    /// <param name="rootInstanceId">The root instance ID of the entity tree.</param>
    /// <returns>The instance of IEntityNodeTree created from the specified root instance ID.</returns>
    public IEntityNodeTree CreateFromRootId(Ulid rootInstanceId);
    /// <summary>
    /// Creates an empty instance of an entity tree.
    /// </summary>
    /// <returns>An instance of IEntityNodeTree.</returns>
    public IEntityNodeTree CreateEmpty();
}
