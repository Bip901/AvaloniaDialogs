using Avalonia.LogicalTree;
using System;

namespace AvaloniaDialogs;

public static class UIUtil
{
    /// <summary>
    /// Recursively traverses the logical tree using depth-first search and returns the first matching element, or null if not found.
    /// </summary>
    public static ILogical? SelectLogicalDescendant(ILogical root, Predicate<ILogical> selector)
    {
        if (selector(root))
            return root;
        foreach (ILogical child in root.LogicalChildren)
        {
            ILogical? result;
            if ((result = SelectLogicalDescendant(child, selector)) != null)
                return result;
        }
        return null;
    }
}
