// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common;
using AterraCore.Contracts.FlexiPlug.Plugin;

namespace AterraCore.Extensions;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public static class LinkedListExtensions {
    public static LinkedListNode<IPluginData>? NextValid(this LinkedListNode<IPluginData> node) {
        ArgumentNullException.ThrowIfNull(node);

        LinkedListNode<IPluginData>? next = node.Next;
        while (next != null && next.Value.Validity == PluginValidity.Invalid) {
            next = next.Next;
        }
        return next;
    }
}