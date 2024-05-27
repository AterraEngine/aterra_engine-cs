// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Contracts.FlexiPlug.Plugin;
namespace Extensions;

using AterraCore.Common.Types.FlexiPlug;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public static class LinkedListExtensions {
    public static LinkedListNode<IPluginDto>? NextValid(this LinkedListNode<IPluginDto> node) {
        ArgumentNullException.ThrowIfNull(node);

        LinkedListNode<IPluginDto>? next = node.Next;
        while (next is { Value.Validity: PluginValidity.Invalid }) {
            next = next.Next;
        }
        return next;
    }
}