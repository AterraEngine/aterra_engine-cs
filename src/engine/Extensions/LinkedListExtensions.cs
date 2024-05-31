// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Common.Types.FlexiPlug;
using AterraCore.Contracts.FlexiPlug.Boot;

namespace Extensions;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public static class LinkedListExtensions {
    public static LinkedListNode<ILoadedPluginDto>? NextValid(this LinkedListNode<ILoadedPluginDto> node) {
        ArgumentNullException.ThrowIfNull(node);

        LinkedListNode<ILoadedPluginDto>? next = node.Next;
        while (next is { Value.Validity: PluginValidity.Invalid }) {
            next = next.Next;
        }
        return next;
    }
}
